using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.ViewModel.Trip;

namespace YemeniDriver.Controllers
{
    /// <summary>
    /// Controller responsible for handling trip-related actions and views.
    /// </summary>
    public class TripController : Controller
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TripController> _logger;

        /// <summary>
        /// Constructor for the TripController.
        /// </summary>
        public TripController(ITripRepository tripRepository, IUserRepository userRepository, ILogger<TripController> logger)
        {
            _tripRepository = tripRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Default action for the TripController.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action to get trips for a specific driver.
        /// </summary>
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
                _logger.LogError(ex, "Error getting driver trips.");
                return View("Error");
            }
        }

        /// <summary>
        /// Action to get trips for a specific passenger.
        /// </summary>
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
                _logger.LogError(ex, "Error getting passenger trips.");
                return View("Error");
            }
        }
    }
}
