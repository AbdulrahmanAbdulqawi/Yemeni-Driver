using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Interfaces;
using YemeniDriver.ViewModel.Trip;
using YemeniDriver.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YemeniDriver.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;

        public TripController(ITripRepository tripRepository, IUserRepository userRepository)
        {
            _tripRepository = tripRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetDriverTrips(string driverId)
        {
            try
            {
                // Retrieve trips for the specified driver
                var trips = await _tripRepository.GetByUserId(driverId, Roles.Driver);

                // Retrieve driver information

                List<GetTripsViewModel> tripsVM = new List<GetTripsViewModel>();

                foreach (var trip in trips)
                {
                    // Retrieve passenger information
                    var passenger = await _userRepository.GetByIdAsync(trip.PassengerId);

                    // Map data to view model
                    var tripVM = new GetTripsViewModel
                    {
                        PassengerName = $"{passenger.FirstName} {passenger.LastName}",
                        StartTime = trip.StartTime,
                        EndTime = trip.EndTime,
                        ApplicationUserId = trip.DriverId,
                        Comment = trip.Comment,
                        DriverRating = trip.DriverRating,
                        Duration = trip.Duration,
                        PassengerRating = trip.PassengerRating,
                        Price = Math.Round(trip.Price, 2),
                        RequestId = trip.RequestId,
                        DropoffLocation = trip.DropoffLocation,
                        PickupLocation = trip.PickupLocation,
                    };

                    tripsVM.Add(tripVM);
                }

                return View(tripsVM);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log or show an error page)
                return View("Error");
            }
        }

        public async Task<IActionResult> GetPassengerTrips(string passengerId)
        {
            try
            {
                // Retrieve trips for the specified driver
                var trips = await _tripRepository.GetByUserId(passengerId, Roles.Passenger);

                // Retrieve driver information

                List<GetTripsViewModel> tripsVM = new List<GetTripsViewModel>();

                foreach (var trip in trips)
                {
                    // Retrieve passenger information
                    var driver = await _userRepository.GetByIdAsync(trip.DriverId);

                    // Map data to view model
                    var tripVM = new GetTripsViewModel
                    {
                        PassengerName = $"{driver.FirstName} {driver.LastName}",
                        StartTime = trip.StartTime,
                        EndTime = trip.EndTime,
                        ApplicationUserId = trip.DriverId,
                        Comment = trip.Comment,
                        DriverRating = trip.DriverRating,
                        Duration = trip.Duration,
                        PassengerRating = trip.PassengerRating,
                        Price = Math.Round(trip.Price, 2),
                        RequestId = trip.RequestId,
                        DropoffLocation = trip.DropoffLocation,
                        PickupLocation = trip.PickupLocation,
                    };

                    tripsVM.Add(tripVM);
                }

                return View(tripsVM);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log or show an error page)
                return View("Error");
            }
        }
    }
}
