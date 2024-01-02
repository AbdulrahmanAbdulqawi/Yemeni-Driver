using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Data.Enums;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Service;
using YemeniDriver.ViewModel.Dashboard;
using YemeniDriver.Data;

namespace YemeniDriver.Controllers
{
    /// <summary>
    /// Controller responsible for handling the dashboard for different user roles.
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<DashboardController> _logger; // Add ILogger

        public DashboardController(
            IDashboardRepository dashboardRepository,
            IHttpContextAccessor httpContextAccessor,
            IRequestRepository requestRepository,
            ILogger<DashboardController> logger) // Inject ILogger in the constructor
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _requestRepository = requestRepository;
            _logger = logger; // Assign ILogger in the constructor
        }

        /// <summary>
        /// Display the passenger dashboard view.
        /// </summary>
        public async Task<IActionResult> PassengerDashboard()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetUserId();
                var passengerDetails = await _dashboardRepository.GetPassengerByIdAsync(userId);

                // Set default location if live location is not available
                if (passengerDetails.LiveLocationLatitude == null || passengerDetails.LiveLocationLongitude == null)
                {
                    passengerDetails.LiveLocationLongitude = 10.5;
                    passengerDetails.LiveLocationLatitude = 10.5;
                }

                // Get a list of drivers
                var drivers = (await _dashboardRepository.GetDrivers()).Where(driver => driver.FirstName != null).ToList();

                // Calculate distance from passenger to each driver and select the closest 5
                var closestDrivers = CalculateClosestDrivers(passengerDetails, drivers, 5);

                var passengerDashboardVM = new PassengerDashboardViewModel(closestDrivers)
                {
                    Id = passengerDetails.Id,
                    FirstName = passengerDetails.FirstName,
                    Location = passengerDetails.Location,
                    Image = passengerDetails.ProfileImageUrl
                };

                return View(passengerDashboardVM);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in PassengerDashboard method.");
                // Optionally handle the exception and return an error view or redirect
                return View("Error");
            }
        }

        /// <summary>
        /// Display the driver dashboard view.
        /// </summary>
        public async Task<IActionResult> DriverDashboard()
        {
            try
            {
                var driverId = _httpContextAccessor.HttpContext.User.GetUserId();
                var driverDetails = await _dashboardRepository.GetDriverByIdAsyncNoTracking(driverId);

                // Retrieve requested requests for the driver
                var requests = await _requestRepository.GetByUserId(driverId, Roles.Driver);
                IEnumerable<Request> requestedRequests = [];

                if (requests != null)
                {
                    requestedRequests = requests.Where(request => request.Status == RequestStatus.Requested);
                }

                var passengers = await _dashboardRepository.GetPassengers();
                var driverDashboardVM = new DriverDashboardViewModel(requestedRequests,passengers.ToList())
                {
                    Id = driverDetails.Id,
                    FirstName = driverDetails.FirstName,
                    Location = driverDetails.Location,
                    Image = driverDetails.ProfileImageUrl
                };

                return View(driverDashboardVM);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in DriverDashboard method.");
                // Optionally handle the exception and return an error view or redirect
                return View("Error");
            }
        }

        /// <summary>
        /// Display the admin dashboard view.
        /// </summary>
        public async Task<IActionResult> AdminDashboard()
        {
            try
            {
                var drivers = await _dashboardRepository.GetDrivers();
                var passengers = await _dashboardRepository.GetPassengers();

                if (drivers == null || passengers == null)
                {
                    // Handle the case where either drivers or passengers (or both) are null
                    if (drivers == null)
                    {
                        drivers = new List<ApplicationUser>();
                    }
                    else if (passengers == null)
                    {
                        passengers = new List<ApplicationUser>();
                    }

                    return View(new AdminDashboardViewModel
                    {
                        Drivers = drivers.ToList(),
                        Passengers = passengers.ToList(),
                    }); // Example: Return an "Error" view
                }

                var adminDashboardVM = new AdminDashboardViewModel
                {
                    Drivers = drivers.ToList(),
                    Passengers = passengers.ToList()
                };

                return View(adminDashboardVM);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in AdminDashboard method.");
                // Optionally handle the exception and return an error view or redirect
                return View("Error");
            }
        }



        // This method calculates the closest drivers to a passenger
        public static List<ApplicationUser> CalculateClosestDrivers(ApplicationUser passenger, List<ApplicationUser> drivers, int count)
        {
            var closestDrivers = new Dictionary<double, ApplicationUser>();

            foreach (var driver in drivers)
            {
                var calculateDistance = DistanceService.CalculateDistance(
                    passenger.LiveLocationLatitude.GetValueOrDefault(),
                    passenger.LiveLocationLongitude.GetValueOrDefault(),
                    driver.LiveLocationLatitude.GetValueOrDefault(),
                    driver.LiveLocationLongitude.GetValueOrDefault());

                while (closestDrivers.ContainsKey(calculateDistance))
                {
                    calculateDistance += 0.0001; // Increment to handle duplicates (adjust as needed)
                }

                closestDrivers.Add(calculateDistance, driver);
            }

            // Order by distance and take the closest N drivers
            var orderedDrivers = closestDrivers.OrderBy(pair => pair.Key).Take(count);

            // Return the ordered list of drivers
            return orderedDrivers.Select(pair => pair.Value).ToList();
        }
    }
}
