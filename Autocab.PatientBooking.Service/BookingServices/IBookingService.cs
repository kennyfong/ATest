using Autocab.PassengerBooking.Service.BookingServices.Requests;
using Autocab.PassengerBooking.Service.BookingServices.Responses;
using System;

namespace Autocab.PassengerBooking.Service.BookingServices
{
    public interface IBookingService
    {
        void AddBooking(AddBookingRequest request);
        GetNextBookingResponse GetPassengerNextBooking(long identificationNumber);
        void CancelBooking(Guid BookingId);
    }
}