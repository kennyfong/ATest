using Autocab.PassengerBooking.Service.Enums;
using System;

namespace Autocab.PassengerBooking.Service.DriverServices.Requests
{
    public class AddDriverRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
    }
}
