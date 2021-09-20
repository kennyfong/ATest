using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.BookingServices;
using Autocab.PassengerBooking.Service.BookingServices.Requests;
using Autocab.PassengerBooking.Service.BookingServices.Validation;
using Autocab.PassengerBooking.Service.Validation;
using System;

namespace Autocab.PassengerBooking.Service.Tests.BookingServices
{
    [TestFixture]
    public class BookingServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PassengerBookingContext _context;
        private Mock<IAddBookingRequestValidator> _validator;

        private BookingService _bookingService;

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
            _validator = _mockRepository.Create<IAddBookingRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _bookingService = new BookingService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddBookingRequest>()))
                .Returns(new AutocabValidationResult(true));
        }

        [Test]
        public void AddBooking_ValidatesRequest()
        {
            //arrange
            var PassengerRequest = _fixture.Create<Passenger>();
            _context.Passenger.Add(PassengerRequest);
            _context.SaveChanges();

            var request = _fixture.Build<AddBookingRequest>()
                .With(o => o.PassengerId, PassengerRequest.Id)
                .Create();

            //act
            _bookingService.AddBooking(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddBooking_NoPassenger_ThrowsArgumentException()
        {
            //act
            var exception = Assert.Throws<ArgumentNullException>(() => _bookingService.GetPassengerNextBooking(1));

            //assert
            exception.Message.Should().Contain("Passenger does not exist");
        }

        [Test]
        public void AddBooking_NoNextBooking_ThrowsArgumentException()
        {
            DateTime startTime = DateTime.Now.AddDays(-1);
            DateTime endTime = DateTime.Now.AddDays(-1).AddMinutes(30);

            var request = _fixture.Create<AddBookingRequest>();
            var Booking = _fixture.Build<Booking>()
                .With(o => o.CreatedDateTime, startTime)
                .With(o => o.CreatedDateTime, endTime)
                .With(o => o.IsCancelled, false)
                .Create();
            _context.Booking.Add(Booking);
            _context.SaveChanges();

            //act
            var exception = Assert.Throws<ArgumentNullException>(() => _bookingService.GetPassengerNextBooking(Booking.PassengerId));

            //assert
            exception.Message.Should().Contain("There are no next booking");
        }

        [Test]
        public void AddBooking_AddsBookingToContextWithGeneratedId()
        {
            //arrange
            var PassengerRequest = _fixture.Create<Passenger>();
            _context.Passenger.Add(PassengerRequest);
            _context.SaveChanges();

            var request = _fixture.Build<AddBookingRequest>()
                .With(o => o.PassengerId, PassengerRequest.Id)
                .Create();

            var expected = new AddBookingRequest
            {
                 DriverId = request.DriverId,
                 PickupDateTime = request.PickupDateTime,
                 PassengerId = PassengerRequest.Id
            };

            //act
            _bookingService.AddBooking(request);

            //assert
            _context.Booking.Should().ContainEquivalentOf(expected, options => options
                .Excluding(Booking => Booking.Id));
        }

        [Test]
        public void GetPassengerNextBooking_ReturnsNextBooking()
        {
            //arrange
            var Booking = _fixture.Build<Booking>()
                .With(o => o.CreatedDateTime, DateTime.UtcNow)
                .With(o => o.PickupDateTime, DateTime.UtcNow.AddDays(1))
                .With(o => o.IsCancelled, false)
                .Create();
            _context.Booking.Add(Booking);
            _context.SaveChanges();

            //act
            var res = _bookingService.GetPassengerNextBooking(Booking.PassengerId);

            //assert
            res.PickupDateTime.Should().Equals(DateTime.UtcNow.AddDays(1));
        }

        [Test]
        public void CancelBooking_ReturnsSuccessful()
        {
            //arrange
            var Booking = _fixture.Build<Booking>()
                .With(o => o.CreatedDateTime, DateTime.UtcNow)
                .With(o => o.PickupDateTime, DateTime.UtcNow.AddDays(1))
                .With(o => o.IsCancelled, false)
                .Create();
            _context.Booking.Add(Booking);
            _context.SaveChanges();

            var expected = new Booking
            {
                DriverId = Booking.DriverId,
                PickupDateTime = Booking.PickupDateTime,
                CreatedDateTime = Booking.CreatedDateTime,
                PassengerId = Booking.PassengerId,
                IsCancelled = true
            };

            //act
            _bookingService.CancelBooking(Booking.Id);

            //assert
            _context.Booking.Should().ContainEquivalentOf(expected, options => options
                .Excluding(Booking => Booking.Driver)
                .Excluding(Booking => Booking.Passenger)
                .Excluding(Booking => Booking.Id));
        }

        [Test]
        public void GetPassengerNextBookingAfterMultipleBookingsAndCancelNext_ReturnsNextBooking()
        {
            //arrange
            var Booking = _fixture.Build<Booking>()
                .With(o => o.CreatedDateTime, DateTime.UtcNow)
                .With(o => o.PickupDateTime, DateTime.Now.AddDays(1))
                .With(o => o.IsCancelled, false)
                .Create();
            _context.Booking.Add(Booking);
            _context.SaveChanges();

            //act
            var res = _bookingService.GetPassengerNextBooking(Booking.PassengerId);

            //assert
            res.PickupDateTime.Should().Equals(DateTime.UtcNow.AddDays(1));
        }

        [Test]
        public void GetPassengerNextBookingAfterOneBookingAndCancellation_ThrowsException()
        {
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = DateTime.Now.AddDays(1).AddMinutes(30);

            //arrange
            var Booking = _fixture.Build<Booking>()
                .With(o => o.CreatedDateTime, startTime)
                .With(o => o.CreatedDateTime, endTime)
                .With(o => o.IsCancelled, true)
                .Create();
            _context.Booking.Add(Booking);

            _context.SaveChanges();

            //assert
            var exception = Assert.Throws<ArgumentNullException>(() => _bookingService.GetPassengerNextBooking(Booking.PassengerId));

            //assert
            exception.Message.Should().Contain("There are no next booking");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
