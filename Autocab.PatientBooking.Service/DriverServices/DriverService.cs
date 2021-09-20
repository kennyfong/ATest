using Microsoft.EntityFrameworkCore;
using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Data.Models;
using Autocab.PassengerBooking.Service.DriverServices.Requests;
using Autocab.PassengerBooking.Service.DriverServices.Responses;
using Autocab.PassengerBooking.Service.DriverServices.Validation;
using Autocab.PassengerBooking.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocab.PassengerBooking.Service.DriverServices
{
    public class DriverService : IDriverService
    {
        private readonly PassengerBookingContext _context;
        private readonly IAddDriverRequestValidator _validator;

        public DriverService(PassengerBookingContext context, IAddDriverRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void AddDriver(AddDriverRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            _context.Driver.Add(new Driver
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

        public GetAllDriversResponse GetAllDrivers()
        {
            var Drivers = _context
                .Driver
                .Select(x => new GetAllDriversResponse.Driver
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DateOfBirth = x.DateOfBirth,
                    Gender = (Gender)x.Gender
                })
                .AsNoTracking()
                .ToList();

            return new GetAllDriversResponse
            {
                Drivers = Drivers
            };
        }
    }
}
