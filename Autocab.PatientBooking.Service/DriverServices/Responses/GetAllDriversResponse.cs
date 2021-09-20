using Autocab.PassengerBooking.Service.Enums;
using System;
using System.Collections.Generic;

namespace Autocab.PassengerBooking.Service.DriverServices.Responses
{
    public class GetAllDriversResponse
    {
        public List<Driver> Drivers { get; set; }

        public class Driver
        {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Gender Gender { get; set; }
            public string Email { get; set; }
        }
    }
}
