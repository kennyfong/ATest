using Autocab.PassengerBooking.Service.BookingServices.Requests;
using Autocab.PassengerBooking.Service.Validation;

namespace Autocab.PassengerBooking.Service.BookingServices.Validation
{
    public interface IAddBookingRequestValidator
    {
        AutocabValidationResult ValidateRequest(AddBookingRequest request);
    }
}
