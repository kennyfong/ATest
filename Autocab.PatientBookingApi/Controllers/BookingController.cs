using Microsoft.AspNetCore.Mvc;
using Autocab.PassengerBooking.Service.BookingServices;
using Autocab.PassengerBooking.Service.BookingServices.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocab.PassengerBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("Passenger/{identificationNumber}/next")]
        public IActionResult GetPassengerNextBooking(long identificationNumber)
        {
            try
            {
                return Ok(_bookingService.GetPassengerNextBooking(identificationNumber));
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(502, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost()]
        public IActionResult AddBooking(AddBookingRequest newBooking)
        {
            try
            {
                _bookingService.AddBooking(newBooking);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete()]
        public IActionResult CancelBooking(Guid bookingId)
        {
            try
            {
                _bookingService.CancelBooking(bookingId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(502, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}