using Autocab.PassengerBooking.Service.DriverServices.Requests;
using Autocab.PassengerBooking.Service.Validation;

namespace Autocab.PassengerBooking.Service.DriverServices.Validation
{
    public interface IAddDriverRequestValidator
    {
        AutocabValidationResult ValidateRequest(AddDriverRequest request);
    }
}