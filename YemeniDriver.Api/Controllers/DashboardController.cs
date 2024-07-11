using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.Service;
using YemeniDriver.Api.ViewModel.Dashboard;

namespace YemeniDriver.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling the dashboard for different user roles.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
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
        /// Display the passenger dashboard.
        /// </summary>
        [HttpGet("getPassengerDashboard")]
        [Authorize(Roles = "Passenger")] // Add this attribute to make the method accessible only by users with the "Passenger" role
        public async Task<IActionResult> GetPassengerDashboard()
        {
            try
            {
                ApplicationUser passengerDetails = null;
                var isUserAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
                if (isUserAuthenticated)
                {
                    var userId = _httpContextAccessor.HttpContext.User.GetUserId();
                    passengerDetails = await _dashboardRepository.GetPassengerByIdAsync(userId);

                }
                else
                {
                    return NotFound("user is not authinticated");

                }

                if (passengerDetails == null)
                {
                    return NotFound("Passenger does not exists");
                }
                // Set default location if live location is not available
                if (passengerDetails.LiveLocationLatitude == null || passengerDetails.LiveLocationLongitude == null)
                {
                    passengerDetails.LiveLocationLongitude = 10.5;
                    passengerDetails.LiveLocationLatitude = 10.5;
                }

                // Get a list of driversif


                var getDrivers = await _dashboardRepository.GetDrivers();

                var driversList = getDrivers?.ToList();

                var closestDrivers = driversList != null
                    ? CalculateClosestDrivers(passengerDetails, driversList, 5)
                    : CalculateClosestDrivers(passengerDetails, null, 5);

                var passengerDashboardVM = new PassengerDashboardViewModel(closestDrivers)
                {
                    Id = passengerDetails.Id,
                    FirstName = passengerDetails.FirstName,
                    Location = passengerDetails.Location,
                    Image = passengerDetails.ProfileImageUrl
                };

                return Ok(passengerDashboardVM);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in GetPassengerDashboard method.");
                // Optionally handle the exception and return an error response
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Display the driver dashboard.
        /// </summary>
        [HttpGet("getDriverDashboard")]
        [Authorize(Roles = "Driver")] // Add this attribute to make the method accessible only by users with the "Passenger" role
        public async Task<IActionResult> GetDriverDashboard()
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

                var driverDashboardVM = new DriverDashboardViewModel(requestedRequests, passengers)
                {
                    Id = driverDetails.Id,
                    FirstName = driverDetails.FirstName,
                    Location = driverDetails.Location,
                    Image = driverDetails.ProfileImageUrl
                };

                return Ok(driverDashboardVM);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in GetDriverDashboard method.");
                // Optionally handle the exception and return an error response
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Display the admin dashboard.
        /// </summary>
        [HttpGet("getAdminDashboard")]
        public async Task<IActionResult> GetAdminDashboard()
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

                    return Ok(new AdminDashboardViewModel
                    {
                        Drivers = drivers.ToList(),
                        Passengers = passengers.ToList(),
                    }); // Example: Return an "Error" response
                }

                var adminDashboardVM = new AdminDashboardViewModel
                {
                    Drivers = drivers.ToList(),
                    Passengers = passengers.ToList()
                };

                return Ok(adminDashboardVM);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in GetAdminDashboard method.");
                // Optionally handle the exception and return an error response
                return StatusCode(500, "Internal Server Error");
            }
        }

        // This method calculates the closest drivers to a passenger
        private List<ApplicationUser> CalculateClosestDrivers(ApplicationUser passenger, List<ApplicationUser>? drivers, int count)
        {
            var closestDrivers = new Dictionary<double, ApplicationUser>();
            if(drivers !=  null)
            {
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


                var orderedDrivers = closestDrivers.OrderBy(pair => pair.Key).Take(count);

                // Return the ordered list of drivers
                return orderedDrivers.Select(pair => pair.Value).ToList();

                // Order by distance and take the closest N drivers
            }
            return null;

        }
    }
}
