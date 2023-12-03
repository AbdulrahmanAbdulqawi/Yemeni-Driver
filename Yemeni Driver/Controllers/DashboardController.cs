using Microsoft.AspNetCore.Mvc;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;
using Yemeni_Driver.Service;
using Yemeni_Driver.ViewModel.Dashboard;

namespace Yemeni_Driver.Controllers
{
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
                FirstName = passengerDetailes.FirstName,
                Location = passengerDetailes.Location,
                Image = passengerDetailes.ProfileImageUrl
                
            };
            
           
            return View(passengerDashboardVM);
        }

        public IActionResult DriverDashboard()
        {
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var driverDetailes = _dashboardRepository.GetDriverByIdAsync(user);
            
            var requests = _requestRepository.GetByStatus(Data.Enums.RequestStatus.Requested).Result;
            var passengers = _dashboardRepository.GetPassengers().Result;

            var driverDashboardVM = new DriverDashboardViewModel(requests, passengers)
            {
                Id = driverDetailes.Result.Id,
                FirstName = driverDetailes.Result.FirstName,
                Location = driverDetailes.Result.Location,
                Image = driverDetailes.Result.ProfileImageUrl
            };


            return View(driverDashboardVM);
        }
    }
}
