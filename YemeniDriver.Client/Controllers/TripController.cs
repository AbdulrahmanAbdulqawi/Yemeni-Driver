using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Interfaces;
using YemeniDriver.ViewModel.Trip;
using YemeniDriver.Interfaces;

namespace YemeniDriver.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDriverAndRequestRepository _driverAndRequestRepository;

        public TripController(ITripRepository tripRepository, IUserRepository userRepository, IDriverAndRequestRepository driverAndRequestRepository)
        {
            _tripRepository = tripRepository;
            _userRepository = userRepository;
            _driverAndRequestRepository = driverAndRequestRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTrips(string driverId)
        {
            var trips = await _tripRepository.GetAll();
            var driverTrips = trips.Where(a => a.DriverId == driverId);
            List<GetTripsViewModel> tripsVM = [];
            foreach (var trip in driverTrips)
            {
                var passenger = await _userRepository.GetByIdAsyncNoTracking(trip.ApplicationUserId);
                //var driverId = await _driverAndRequestRepository.GetDriverIdByRequestId(trip.RequestId);
                var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);

                var tripVM = new GetTripsViewModel
                {
                    PassengerName = passenger.FirstName + " " +  passenger.LastName,
                    //DriverName = driver.FirstName + " " + driver.LastName,
                    StartTime = trip.StartTime,
                    EndTime = trip.EndTime,
                    ApplicationUserId = trip.ApplicationUserId,
                    Comment = trip.Comment,
                    DriverRating = trip.DriverRating,
                    Duration = trip.Duration,
                    PassengerRating = trip.PassengerRating,
                    Price = Math.Round(trip.Price, 2),
                    RequestId = trip.RequestId,
                };
                tripsVM.Add(tripVM);
            }
            
            return View(tripsVM);
        }
    }
}
