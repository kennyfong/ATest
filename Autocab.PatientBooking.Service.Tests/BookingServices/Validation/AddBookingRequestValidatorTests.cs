using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.BookingServices.Requests;
using Autocab.PassengerBooking.Service.BookingServices.Validation;
using System;

namespace Autocab.PassengerBooking.Service.Tests.BookingServices.Validation
{
    [TestFixture]
    public class AddBookingRequestValidatorTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PassengerBookingContext _context;

        private AddBookingRequestValidator _addBookingRequestValidator;

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

            // default context
            SetupDefaultContext();

            // Sut instantiation
            _addBookingRequestValidator = new AddBookingRequestValidator(
                _context
            );
        }

        private void SetupDefaultContext()
        {
            var Booking = _fixture.Build<Booking>()
                .With(b => b.DriverId, 1)
                .With(b => b.CreatedDateTime, new DateTime(2022,3,23,0,0,0))
                .With(b => b.PickupDateTime, new DateTime(2022,3,23,1,0,0))
                .Create();
            _context.Booking.Add(Booking);
            _context.SaveChanges();
        }

        [Test]
        public void ValidateRequest_AllChecksPass_ReturnsPassedValidationResult()
        {
            //arrange
            var request = GetValidRequest();
            request.DriverId = 2;

            //act
            var res = _addBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [Test]
        public void ValidateRequest_AllChecksPassWithExistingDriverBooked_ReturnsPassedValidationResult()
        {
            //arrange
            var request = GetValidRequest();
            request.DriverId = 1;
            request.PickupDateTime = new DateTime(2022,3,23,2,30,00);

            //act
            var res = _addBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [TestCase("2020-03-23 01:00:00")]
        public void ValidateRequest_BookingInThePast_ReturnsFailedValidationResult(DateTime bookingPickUpDateTime)
        {
            //arrange
            var request = GetValidRequest();
            request.PickupDateTime = bookingPickUpDateTime;

            //act
            var res = _addBookingRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("Pick Up Time must not be set in the past");
        }

        private AddBookingRequest GetValidRequest()
        {
            var request = _fixture.Build<AddBookingRequest>()
                .With(b => b.PickupDateTime, DateTime.Now.AddDays(1))
                .Create();
            return request;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
