using Autocab.PassengerBooking.Service.Enums;
using System;
using System.Collections.Generic;

namespace Autocab.PassengerBooking.Service.PassengerServices.Responses
{
    public class GetAllPassengersResponse
    {
        public List<Passenger> Passengers { get; set; }

        public class Passenger
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
