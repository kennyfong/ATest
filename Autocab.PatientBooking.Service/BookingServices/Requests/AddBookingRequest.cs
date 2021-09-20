using Autocab.PassengerBooking.Data.Models;
using System;

namespace Autocab.PassengerBooking.Service.BookingServices.Requests
{
    public class AddBookingRequest
    {
        public Guid Id { get; set; }
        public DateTime PickupDateTime { get; set; }
        public long PassengerId { get; set; }
        public long DriverId { get; set; }
    }
}
