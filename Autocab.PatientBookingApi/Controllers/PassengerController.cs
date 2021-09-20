using Microsoft.AspNetCore.Mvc;
using Autocab.PassengerBooking.Service.PassengerServices;
using Autocab.PassengerBooking.Service.PassengerServices.Requests;
using System;

namespace Autocab.PassengerBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _PassengerService;

        public PassengerController(IPassengerService PassengerService)
        {
            _PassengerService = PassengerService;
        }

        [HttpGet()]
        public IActionResult GetAllPassengers()
        {
            try
            {
                return Ok(_PassengerService.GetAllPassengers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost()]
        public IActionResult AddPassenger(AddPassengerRequest request)
        {
            try
            {
                _PassengerService.AddPassenger(request);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}