using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.DriverServices;
using Autocab.PassengerBooking.Service.DriverServices.Requests;
using Autocab.PassengerBooking.Service.DriverServices.Responses;
using Autocab.PassengerBooking.Service.DriverServices.Validation;
using Autocab.PassengerBooking.Service.Enums;
using Autocab.PassengerBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocab.PassengerBooking.Service.Tests.DriverServices
{
    [TestFixture]
    public class DriverServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PassengerBookingContext _context;
        private Mock<IAddDriverRequestValidator> _validator;

        private DriverService _DriverService;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PassengerBookingContext(new DbContextOptionsBuilder<PassengerBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _validator = _mockRepository.Create<IAddDriverRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _DriverService = new DriverService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddDriverRequest>()))
                .Returns(new AutocabValidationResult(true));
        }

        [Test]
        public void AddDriver_ValidatesRequest()
        {
            //arrange
            var request = _fixture.Create<AddDriverRequest>();

            //act
            _DriverService.AddDriver(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddDriver_ValidatorFails_ThrowsArgumentException()
        {
            //arrange
            var failedValidationResult = new AutocabValidationResult(false, _fixture.Create<string>());

            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddDriverRequest>())).Returns(failedValidationResult);

            //act
            var exception = Assert.Throws<ArgumentException>(() => _DriverService.AddDriver(_fixture.Create<AddDriverRequest>()));

            //assert
            exception.Message.Should().Be(failedValidationResult.Errors.First());
        }

        [Test]
        public void AddDriver_AddsDriverToContextWithGeneratedId()
        {
            //arrange
            var request = _fixture.Create<AddDriverRequest>();

            var expected = new Driver
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
            _DriverService.AddDriver(request);

            //assert
            _context.Driver.Should().ContainEquivalentOf(expected, options => options
                .Excluding(Driver => Driver.Id)
                .Excluding(Driver => Driver.Created));
        }

        [Test]
        public void GetAllDrivers_NoDrivers_ReturnsEmptyList()
        {
            //arrange

            //act
            var res = _DriverService.GetAllDrivers();

            //assert
            res.Drivers.Should().BeEmpty();
        }

        [Test]
        public void GetAllDrivers_ReturnsMappedDriverList()
        {
            //arrange
            var Driver = _fixture.Create<Driver>();
            _context.Driver.Add(Driver);
            _context.SaveChanges();

            var expected = new GetAllDriversResponse
            {
                Drivers = new List<GetAllDriversResponse.Driver>
                {
                    new GetAllDriversResponse.Driver
                    {
                        Id = Driver.Id,
                        FirstName = Driver.FirstName,
                        LastName = Driver.LastName,
                        Gender = (Gender)Driver.Gender,
                        DateOfBirth = Driver.DateOfBirth,
                        Email = Driver.Email
                    }
                }
            };

            //act
            var res = _DriverService.GetAllDrivers();

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
