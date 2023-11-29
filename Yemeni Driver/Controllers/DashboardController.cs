using Microsoft.AspNetCore.Mvc;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Service;
using Yemeni_Driver.ViewModel.Dashboard;

namespace Yemeni_Driver.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PassengerDashboard()
        {
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var passengerDetailes = _dashboardRepository.GetPassengerByIdAsync(user);
            var drivers = _dashboardRepository.GetDrivers().Result.ToList();


            var passengerDashboardVM = new PassengerDashboardViewModel(drivers)
            {
                FirstName = passengerDetailes.Result.FirstName,
                Location = passengerDetailes.Result.Location,
                
            };
            
           
            return View(passengerDashboardVM);
        }

        public IActionResult DriverDashboard()
        {
            var user = _httpContextAccessor.HttpContext.User.GetUserId();
            var DriverDetailes = _dashboardRepository.GetDriverByIdAsync(user);

            var driverDashboardVM = new DriverDashboardViewModel
            {
                FirstName = DriverDetailes.Result.FirstName,
                Location = DriverDetailes.Result.Location,

            };


            return View(driverDashboardVM);
        }
    }
}
