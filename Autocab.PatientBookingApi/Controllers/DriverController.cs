using Microsoft.AspNetCore.Mvc;
using Autocab.PassengerBooking.Service.DriverServices;
using Autocab.PassengerBooking.Service.DriverServices.Requests;
using System;

namespace Autocab.PassengerBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _DriverService;

        public DriverController(IDriverService DriverService)
        {
            _DriverService = DriverService;
        }

        [HttpGet()]
        public IActionResult GetAllDrivers()
        {
            try
            {
                return Ok(_DriverService.GetAllDrivers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost()]
        public IActionResult AddDriver(AddDriverRequest request)
        {
            try
            {
                _DriverService.AddDriver(request);
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