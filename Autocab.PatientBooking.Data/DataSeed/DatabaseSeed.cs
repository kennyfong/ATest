using Autocab.PassengerBooking.Data.Models;
using System;
using System.Collections.Generic;

namespace Autocab.PassengerBooking.Data.DataSeed
{
    /// <summary>
    /// You can ignore this class. Because we're using an in memory database we wanted to seed the db with data to enable you to test your application
    /// </summary>
    public class DatabaseSeed
    {
        private readonly PassengerBookingContext _context;

        public DatabaseSeed(PassengerBookingContext context)
        {
            _context = context;
        }

        public void SeedDatabase()
        {
            var Drivers = AddDrivers();
            var Passengers = AddPassengers();
            var Bookings = AddBookings();

            LinkBookingsToDrivers(Bookings, Drivers);
            LinkBookingsToPassengers(Bookings, Passengers);
        }


        private List<Booking> AddBookings()
        {
            var Bookings = new List<Booking>
            {
                new Booking
                {
                    Id = Guid.Parse("683074b8-44c9-468b-9288-dfafa1e533c9"),
                    CreatedDateTime = new DateTime(2020, 1, 11, 12, 15, 00),
                    PickupDateTime = new DateTime(2020, 1, 11, 12, 30, 00)
                },
                new Booking
                {
                    Id = Guid.Parse("57706153-7600-41fd-8a5e-dc60a584e21c"),
                    CreatedDateTime = new DateTime(2020, 1, 11, 12, 30, 00),
                    PickupDateTime = new DateTime(2020, 1, 11, 12, 45, 00)
                },
                new Booking
                {
                    Id = Guid.Parse("b6aad474-5b5d-42b7-a274-1a94f74ff69a"),
                    CreatedDateTime = new DateTime(2020, 1, 11, 14, 15, 00),
                    PickupDateTime = new DateTime(2020, 1, 11, 14, 30, 00)
                },
                new Booking
                {
                    Id = Guid.Parse("b67c2730-0c12-4236-9b1a-d5b1d22db247"),
                    CreatedDateTime = new DateTime(2021, 1, 11, 12, 15, 00),
                    PickupDateTime = new DateTime(2021, 1, 11, 12, 30, 00)
                },
                new Booking
                {
                    Id = Guid.Parse("31924ff1-c64e-4e1e-977e-704abc062aa4"),
                    CreatedDateTime = new DateTime(2021, 1, 12, 12, 15, 00),
                    PickupDateTime = new DateTime(2021, 1, 12, 12, 30, 00)
                }
            };

            _context.Booking.AddRange(Bookings);
            _context.SaveChanges();

            return Bookings;
        }

        private List<Passenger> AddPassengers()
        {
            var Passengers = new List<Passenger>
            {
                new Passenger
                {
                    Id = 100,
                    Gender = 1,
                    FirstName = "Bill",
                    LastName = "Bagly",
                    Email = "BToTheB@gmail.com",
                    DateOfBirth = new DateTime(1912, 1, 17),
                    Created = DateTime.UnixEpoch
                },
                new Passenger
                {
                    Id = 173,
                    Gender = 1,
                    FirstName = "Philbert",
                    LastName = "McPlop",
                    Email = "ThePIsSilent@gmail.com",
                    DateOfBirth = new DateTime(1968, 4, 7),
                    Created = DateTime.UnixEpoch
                },
                new Passenger
                {
                    Id = 159,
                    Gender = 1,
                    FirstName = "Stephen",
                    LastName = "Fry",
                    Email = "TheRealStephenFry@gmail.com",
                    DateOfBirth = new DateTime(1957, 8, 24),
                    Created = DateTime.UnixEpoch
                }
            };

            _context.Passenger.AddRange(Passengers);
            _context.SaveChanges();

            return Passengers;
        }

        private List<Driver> AddDrivers()
        {
            var Drivers = new List<Driver>
            {
                new Driver()
                {
                    Id = 1,
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Email = "DrMg@docworld.com",
                    FirstName = "Mac",
                    LastName = "Guffin",
                    Gender = 1,
                    Created = DateTime.UtcNow
                },
                new Driver()
                {
                    Id = 2,
                    DateOfBirth = new DateTime(1975, 5, 3),
                    Email = "DrBlop@docworld.com",
                    FirstName = "Betty",
                    LastName = "Blop",
                    Gender = 0,
                    Created = DateTime.UtcNow
                },
                new Driver()
                {
                    Id = 3,
                    DateOfBirth = new DateTime(1990, 10, 12),
                    Email = "L33tFoosBallPlayer69@docworld.com",
                    FirstName = "Lindsay",
                    LastName = "Mcowat",
                    Gender = 0,
                    Created = DateTime.UtcNow
                }
            };

            _context.Driver.AddRange(Drivers);
            _context.SaveChanges();

            return Drivers;
        }

        private void LinkBookingsToDrivers(List<Booking> Bookings, List<Driver> Drivers)
        {
            var count = 0;
            foreach (var Booking in Bookings)
            {
                Booking.DriverId = Drivers[count++ % Drivers.Count].Id;
            }

            _context.SaveChanges();
        }

        private void LinkBookingsToPassengers(List<Booking> Bookings, List<Passenger> Passengers)
        {
            var count = 0;
            foreach (var Booking in Bookings)
            {
                Booking.PassengerId = Passengers[count++ % Passengers.Count].Id;
            }

            _context.SaveChanges();
        }
    }
}
