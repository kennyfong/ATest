using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Service.DriverServices.Requests;
using Autocab.PassengerBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Autocab.PassengerBooking.Service.DriverServices.Validation
{
    public class AddDriverRequestValidator : IAddDriverRequestValidator
    {
        private readonly PassengerBookingContext _context;

        public AddDriverRequestValidator(PassengerBookingContext context)
        {
            _context = context;
        }

        public AutocabValidationResult ValidateRequest(AddDriverRequest request)
        {
            var result = new AutocabValidationResult(true);

            if (MissingRequiredFields(request, ref result))
                return result;

            if (WrongFormatFields(request, ref result))
                return result;

            if (DriverAlreadyInDb(request, ref result))
                return result;

            return result;
        }

        public bool MissingRequiredFields(AddDriverRequest request, ref AutocabValidationResult result)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(request.FirstName))
                errors.Add("FirstName must be populated");

            if (string.IsNullOrEmpty(request.LastName))
                errors.Add("LastName must be populated");

            if (string.IsNullOrEmpty(request.Email))
                errors.Add("Email must be populated");

            if (errors.Any())
            {
                result.PassedValidation = false;
                result.Errors.AddRange(errors);
                return true;
            }

            return false;
        }

        public bool WrongFormatFields(AddDriverRequest request, ref AutocabValidationResult result)
        {
            var errors = new List<string>();

            // Can be replaced with MailAddress.TryCreate in .Net 5 to avoid using ugly Try/Catch
            try
            {
                MailAddress m = new MailAddress(request.Email);
            }
            catch (FormatException)
            {
                errors.Add("Email must be a valid email address");
            }                

            if (errors.Any())
            {
                result.PassedValidation = false;
                result.Errors.AddRange(errors);
                return true;
            }

            return false;
        }

        private bool DriverAlreadyInDb(AddDriverRequest request, ref AutocabValidationResult result)
        {
            if (_context.Driver.Any(x => x.Email == request.Email))
            {
                result.PassedValidation = false;
                result.Errors.Add("A Driver with that email address already exists");
                return true;
            }

            return false;
        }
    }
}
