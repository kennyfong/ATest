using Autocab.PassengerBooking.Service.Enums;
using System;

namespace Autocab.PassengerBooking.Service.PassengerServices.Requests
{
    public class AddPassengerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
    }
}
