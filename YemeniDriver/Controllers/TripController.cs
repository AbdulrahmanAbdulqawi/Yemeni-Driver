using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Interfaces;
using YemeniDriver.ViewModel.Trip;
using YemeniDriver.Interfaces;
using YemeniDriver.Data;

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

        public async Task<IActionResult> GetTrips(string driverId)
        {
            var trips = await _tripRepository.GetByUserId(driverId, Roles.Driver);
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);


            List<GetTripsViewModel> tripsVM = [];
            foreach (var trip in trips)
            {
                var passenger = await _userRepository.GetByIdAsync(trip.PassengerId);
                //var driverId = await _driverAndRequestRepository.GetDriverIdByRequestId(trip.RequestId);

                var tripVM = new GetTripsViewModel
                {
                    PassengerName = passenger.FirstName + " " +  passenger.LastName,
                    //DriverName = driver.FirstName + " " + driver.LastName,
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
    }
}
