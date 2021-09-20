using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Service.PassengerServices.Requests;
using Autocab.PassengerBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Autocab.PassengerBooking.Service.PassengerServices.Validation
{
    public class AddPassengerRequestValidator : IAddPassengerRequestValidator
    {
        private readonly PassengerBookingContext _context;

        public AddPassengerRequestValidator(PassengerBookingContext context)
        {
            _context = context;
        }

        public AutocabValidationResult ValidateRequest(AddPassengerRequest request)
        {
            var result = new AutocabValidationResult(true);

            if (MissingRequiredFields(request, ref result))
                return result;

            if (PassengerAlreadyInDb(request, ref result))
                return result;

            if (WrongFormatFields(request, ref result))
                return result;

            return result;
        }

        private bool MissingRequiredFields(AddPassengerRequest request, ref AutocabValidationResult result)
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

        private bool WrongFormatFields(AddPassengerRequest request, ref AutocabValidationResult result)
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

        private bool PassengerAlreadyInDb(AddPassengerRequest request, ref AutocabValidationResult result)
        {
            if (_context.Passenger.Any(x => x.Email == request.Email))
            {
                result.PassedValidation = false;
                result.Errors.Add("A Passenger with that email address already exists");
                return true;
            }

            return false;
        }
    }
}
