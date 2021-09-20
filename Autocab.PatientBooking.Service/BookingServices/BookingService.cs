using Microsoft.EntityFrameworkCore;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.BookingServices.Requests;
using Autocab.PassengerBooking.Service.BookingServices.Responses;
using Autocab.PassengerBooking.Service.BookingServices.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocab.PassengerBooking.Service.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly PassengerBookingContext _context;
        private readonly IAddBookingRequestValidator _validator;

        public BookingService(PassengerBookingContext context, IAddBookingRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void AddBooking(AddBookingRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            var bookingId = new Guid();
            var bookingPickupDateTime = request.PickupDateTime;
            var bookingPassengerId = request.PassengerId;
            var bookingPassenger = _context.Passenger.FirstOrDefault(x => x.Id == request.PassengerId);
            var bookingDriverId = request.DriverId;
            var bookingDriver = _context.Driver.FirstOrDefault(x => x.Id == request.DriverId);

            var myBooking = new Booking
            {
                Id = bookingId,
                CreatedDateTime = DateTime.UtcNow,
                PickupDateTime = bookingPickupDateTime,
                PassengerId = bookingPassengerId,
                DriverId = bookingDriverId,
                Passenger = bookingPassenger,
                Driver = bookingDriver
            };

            _context.Booking.AddRange(new List<Booking> { myBooking });
            _context.SaveChanges();
        }

        public void CancelBooking(Guid BookingId)
        {
            var booking = _context.Booking.FirstOrDefault(x => x.Id == BookingId);
            
            if (booking == null)
            {
                throw new ArgumentNullException("Booking does not exist");
            }

            booking.IsCancelled = true;

            _context.Booking.Update(booking);
            _context.SaveChanges();
        }

        public GetNextBookingResponse GetPassengerNextBooking(long identificationNumber)
        {
            var booking = _context.Booking.OrderBy(x => x.CreatedDateTime).ToList();

            if (booking.Where(x => x.Passenger.Id == identificationNumber).Count() == 0)
            {
                throw new ArgumentNullException("Passenger does not exist");
            }
            else
            {
                var bookings2 = booking.Where(x => x.PassengerId == identificationNumber);
                if (bookings2.Where(x => x.CreatedDateTime > DateTime.UtcNow && !x.IsCancelled).Count() == 0)
                {
                    throw new ArgumentNullException("There are no next booking");
                }
                else
                {
                    var bookings3 = bookings2.Where(x => x.CreatedDateTime > DateTime.UtcNow);
                    return new GetNextBookingResponse()
                    { 
                        Id = bookings3.First().Id,
                        DriverId = bookings3.First().DriverId,
                        PickupDateTime = bookings3.First().PickupDateTime
                    };
                }
            }
        }
    }
}
