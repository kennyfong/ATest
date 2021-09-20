using System;
using System.Collections.Generic;

namespace Autocab.PassengerBooking.Data.Models
{
    public class Driver
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public DateTime Created { get; set; }
    }
}
