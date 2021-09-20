using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.Enums;
using Autocab.PassengerBooking.Service.PassengerServices;
using Autocab.PassengerBooking.Service.PassengerServices.Requests;
using Autocab.PassengerBooking.Service.PassengerServices.Responses;
using Autocab.PassengerBooking.Service.PassengerServices.Validation;
using Autocab.PassengerBooking.Service.Validation;

namespace Autocab.PassengerBooking.Service.Tests.PassengerServices
{
    [TestFixture]
    public class PassengerServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PassengerBookingContext _context;
        private Mock<IAddPassengerRequestValidator> _validator;

        private PassengerService _PassengerService;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PassengerBookingContext(new DbContextOptionsBuilder<PassengerBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _validator = _mockRepository.Create<IAddPassengerRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _PassengerService = new PassengerService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddPassengerRequest>()))
                .Returns(new AutocabValidationResult(true));
        }

        [Test]
        public void AddPassenger_ValidatesRequest()
        {
            //arrange
            var request = _fixture.Create<AddPassengerRequest>();

            //act
            _PassengerService.AddPassenger(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddPassenger_ValidatorFails_ThrowsArgumentException()
        {
            //arrange
            var failedValidationResult = new AutocabValidationResult(false, _fixture.Create<string>());

            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddPassengerRequest>())).Returns(failedValidationResult);

            //act
            var exception = Assert.Throws<ArgumentException>(() =>_PassengerService.AddPassenger(_fixture.Create<AddPassengerRequest>()));

            //assert
            exception.Message.Should().Be(failedValidationResult.Errors.First());
        }

        [Test]
        public void AddPassenger_AddsPassengerToContextWithGeneratedId()
        {
            //arrange
            var request = _fixture.Create<AddPassengerRequest>();

            var expected = new Passenger
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Bookings = new List<Booking>(),
                Created = DateTime.UtcNow
            };

            //act
            _PassengerService.AddPassenger(request);

            //assert
            _context.Passenger.Should().ContainEquivalentOf(expected, options => options
                .Excluding(Passenger => Passenger.Id)
                .Excluding(Passenger => Passenger.Created));
        }

        [Test]
        public void GetAllPassengers_NoPassengers_ReturnsEmptyList()
        {
            //arrange

            //act
            var res  = _PassengerService.GetAllPassengers();

            //assert
            res.Passengers.Should().BeEmpty();
        }

        [Test]
        public void GetAllPassengers_ReturnsMappedPassengerList()
        {
            //arrange
            var Passenger = _fixture.Create<Passenger>();
            _context.Passenger.Add(Passenger);
            _context.SaveChanges();

            var expected = new GetAllPassengersResponse
            {
                Passengers = new List<GetAllPassengersResponse.Passenger>
                {
                    new GetAllPassengersResponse.Passenger
                    {
                        Id = Passenger.Id,
                        FirstName = Passenger.FirstName,
                        LastName = Passenger.LastName,
                        Gender = (Gender)Passenger.Gender,
                        DateOfBirth = Passenger.DateOfBirth,
                        Email = Passenger.Email
                    }
                }
            };

            //act
            var res = _PassengerService.GetAllPassengers();

            //assert
            res.Should().BeEquivalentTo(expected);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
