using Autocab.PassengerBooking.Service.DriverServices.Requests;
using Autocab.PassengerBooking.Service.DriverServices.Responses;

namespace Autocab.PassengerBooking.Service.DriverServices
{
    public interface IDriverService
    {
        void AddDriver(AddDriverRequest request);
        GetAllDriversResponse GetAllDrivers();
    }
}