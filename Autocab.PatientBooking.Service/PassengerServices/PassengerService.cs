using Microsoft.EntityFrameworkCore;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.Enums;
using Autocab.PassengerBooking.Service.PassengerServices.Requests;
using Autocab.PassengerBooking.Service.PassengerServices.Responses;
using Autocab.PassengerBooking.Service.PassengerServices.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocab.PassengerBooking.Service.PassengerServices
{
    public class PassengerService : IPassengerService
    {
        private readonly PassengerBookingContext _context;
        private readonly IAddPassengerRequestValidator _validator;

        public PassengerService(PassengerBookingContext context, IAddPassengerRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void AddPassenger(AddPassengerRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            _context.Passenger.Add(new Passenger
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Bookings = new List<Booking>(),
                Created = DateTime.UtcNow
            });

            _context.SaveChanges();
        }

        public GetAllPassengersResponse GetAllPassengers()
        {
            var Passengers = _context
                .Passenger
                .Select(x => new GetAllPassengersResponse.Passenger
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DateOfBirth = x.DateOfBirth,
                    Gender = (Gender)x.Gender
                })
                .ToList();

            return new GetAllPassengersResponse
            {
                Passengers = Passengers
            };
        }
    }
}
