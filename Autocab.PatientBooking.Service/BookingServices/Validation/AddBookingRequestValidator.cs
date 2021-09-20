using Autocab.PassengerBooking.Data;
using Autocab.PassengerBooking.Service.BookingServices.Requests;
using Autocab.PassengerBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocab.PassengerBooking.Service.BookingServices.Validation
{
    public class AddBookingRequestValidator : IAddBookingRequestValidator
    {
        private readonly PassengerBookingContext _context;

        public AddBookingRequestValidator(PassengerBookingContext context)
        {
            _context = context;
        }

        public AutocabValidationResult ValidateRequest(AddBookingRequest request)
        {
            var result = new AutocabValidationResult(true);

            if (CheckBookingInPast(request, ref result))
                return result;

            return result;
        }

        public bool CheckBookingInPast(AddBookingRequest request, ref AutocabValidationResult result)
        {
            var errors = new List<string>();

            if (request.PickupDateTime < DateTime.Now)
                errors.Add("Pick Up Time must not be set in the past");

            if (errors.Any())
            {
                result.PassedValidation = false;
                result.Errors.AddRange(errors);
                return true;
            }

            return false;
        }
    }
}
