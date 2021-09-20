using System;

namespace Autocab.PassengerBooking.Service.BookingServices.Responses
{
    public class GetNextBookingResponse
    {
        public Guid Id { get; set; }
        public long DriverId { get; set; }
        public DateTime PickupDateTime { get; set; }
    }
}
