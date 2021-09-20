using System.Collections.Generic;

namespace Autocab.PassengerBooking.Service.Validation
{
    public class AutocabValidationResult
    {
        public bool PassedValidation { get; set; }
        public List<string> Errors { get; set; }

        public AutocabValidationResult(bool passedValidation)
        {
            PassedValidation = passedValidation;
            Errors = new List<string>();
        }

        public AutocabValidationResult(bool passedValidation, string error)
        {
            PassedValidation = passedValidation;
            Errors = new List<string> { error };
        }

        public AutocabValidationResult(bool passedValidation, List<string> errors)
        {
            PassedValidation = passedValidation;
            Errors = errors;
        }
    }
}
