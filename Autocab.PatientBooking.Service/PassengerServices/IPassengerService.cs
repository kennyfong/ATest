using Autocab.PassengerBooking.Service.PassengerServices.Requests;
using Autocab.PassengerBooking.Service.PassengerServices.Responses;

namespace Autocab.PassengerBooking.Service.PassengerServices
{
    public interface IPassengerService
    {
        void AddPassenger(AddPassengerRequest request);
        GetAllPassengersResponse GetAllPassengers();
    }
}