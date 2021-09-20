using Microsoft.Extensions.DependencyInjection;
using Autocab.PassengerBooking.Service.BookingServices;
using Autocab.PassengerBooking.Service.BookingServices.Validation;
using Autocab.PassengerBooking.Service.DriverServices;
using Autocab.PassengerBooking.Service.DriverServices.Validation;
using Autocab.PassengerBooking.Service.PassengerServices;
using Autocab.PassengerBooking.Service.PassengerServices.Validation;

namespace Autocab.PassengerBooking.Service.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterPassengerBookingServices(this IServiceCollection collection)
        {
            collection.AddScoped<IPassengerService, PassengerService>();
            collection.AddScoped<IAddPassengerRequestValidator, AddPassengerRequestValidator>();

            collection.AddScoped<IDriverService, DriverService>();
            collection.AddScoped<IAddDriverRequestValidator, AddDriverRequestValidator>();

            collection.AddScoped<IBookingService, BookingService>();
            collection.AddScoped<IAddBookingRequestValidator, AddBookingRequestValidator>();
        }
    }
}
