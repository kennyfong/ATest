using Autocab.PassengerBooking.Service.PassengerServices.Requests;
using Autocab.PassengerBooking.Service.Validation;

namespace Autocab.PassengerBooking.Service.PassengerServices.Validation
{
    public interface IAddPassengerRequestValidator
    {
        AutocabValidationResult ValidateRequest(AddPassengerRequest request);
    }
}