using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.ViewModel.Dashboard;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Service;
using YemeniDriver.ViewModel.Dashboard;

namespace YemeniDriver.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestRepository _requestRepository;
        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IRequestRepository requestRepository)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _requestRepository = requestRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PassengerDashboard()
        {
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var passengerDetailes = await _dashboardRepository.GetPassengerByIdAsync(user);
            
            if(passengerDetailes.LiveLocationLatitude == null || passengerDetailes.LiveLocationLongitude == null)
            {
                passengerDetailes.LiveLocationLongitude = 10.5;
                passengerDetailes.LiveLocationLatitude = 10.5;
            }
            var drivers = _dashboardRepository.GetDrivers().Result.Where(a => a.FirstName != null).ToList();
            
            Dictionary<double, ApplicationUser> closestDriver = [];
            foreach (var driver in drivers) {
                var claculateDistance = DistanceService
              .CalculateDistance((double)passengerDetailes.LiveLocationLatitude, (double)passengerDetailes.LiveLocationLongitude,
              (double)driver.LiveLocationLatitude, (double)driver.LiveLocationLongitude);
                if(closestDriver.ContainsKey(claculateDistance))
                {
                    claculateDistance++;
                }
                closestDriver.Add(claculateDistance, driver);
            }

            closestDriver.OrderByDescending(a => a.Key).Take(5);

            var orderdDrivers = new List<ApplicationUser>();

            foreach (var item in closestDriver.Values)
            {
                orderdDrivers.Add(item);
            }

            var passengerDashboardVM = new PassengerDashboardViewModel(orderdDrivers)
            {
                Id = passengerDetailes.Id,
                FirstName = passengerDetailes.FirstName,
                Location = passengerDetailes.Location,
                Image = passengerDetailes.ProfileImageUrl
                
            };
            
           
            return View(passengerDashboardVM);
        }

        public async Task<IActionResult> DriverDashboard()
        {
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var driverDetails = await _dashboardRepository.GetDriverByIdAsync(user);

            // Ensure the above asynchronous call is awaited to get the actual result

            var requests = await _requestRepository.GetByStatus(Data.Enums.RequestStatus.Requested);
            var driverRequests = requests.Where(a => a.DriverID == user);

            var driverDashboardVM = new DriverDashboardViewModel(
                driverRequests,
                await _dashboardRepository.GetPassengers())
                {
                    Id = driverDetails.Id,
                    FirstName = driverDetails.FirstName,
                    Location = driverDetails.Location,
                    Image = driverDetails.ProfileImageUrl
                };

            return View(driverDashboardVM);
        }

        public async Task<IActionResult> AdminDashboard()
        {
            var drivers = await _dashboardRepository.GetDrivers();
            var passengers = await _dashboardRepository.GetPassengers();


            var adminDashboardVM = new AdminDashboardViewModel
            {
                Drivers = drivers.ToList(),
                Passengers = passengers.ToList()
            };

            return View(adminDashboardVM);
        }

    }
}
