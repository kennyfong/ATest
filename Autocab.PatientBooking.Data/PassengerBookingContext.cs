using Microsoft.EntityFrameworkCore;
using Autocab.PassengerBooking.Data.Models;

namespace Autocab.PassengerBooking.Data
{
    public class PassengerBookingContext : DbContext
    {
        public PassengerBookingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Booking> Booking { get; set; }
        public DbSet<Passenger> Passenger { get; set; }
        public DbSet<Driver> Driver { get; set; }
    }
}
