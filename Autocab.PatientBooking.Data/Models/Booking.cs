using System;

namespace Autocab.PassengerBooking.Data.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime PickupDateTime { get; set; }
        public virtual long PassengerId { get; set; }
        public virtual long DriverId { get; set; }
        public virtual Passenger Passenger { get; set; }
        public virtual Driver Driver { get; set; }
        public bool IsCancelled { get; set; } = false;
    }
}
