using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Service;
using YemeniDriver.ViewModel.Request;

namespace YemeniDriver.Controllers
{
    /// <summary>
    /// Controller responsible for handling ride requests and interactions.
    /// </summary>
    [ApiController]
    [Route("api/request/")]
    public class RequestController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITripRepository _tripRepository;
        private readonly INotyfService _notyf;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RequestController> _logger;

        /// <summary>
        /// Constructor for the RequestController.
        /// </summary>
        public RequestController(
            IRequestRepository requestRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ITripRepository tripRepository,
            INotyfService notyf,
            IHubContext<NotificationHub> hubContext,
            IUserRepository userRepository,
            ILogger<RequestController> logger)
        {
            _requestRepository = requestRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tripRepository = tripRepository;
            _notyf = notyf;
            _hubContext = hubContext;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to create a new ride request.
        /// </summary>
        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestViewModel createRequestVM)
        {
            try
            {
                var passengerId = _httpContextAccessor.HttpContext.User.GetUserId();
                var user = await _userRepository.GetByIdAsyncNoTracking(passengerId);
                var requests = await _requestRepository.GetByUserId(passengerId, Roles.Passenger);

                if (requests != null && requests.Any(a => a.Status == Data.Enums.RequestStatus.Requested))
                {
                    _notyf.Information("Request In Progress");
                    return RedirectToAction("PassengerDashboard", "Dashboard");
                }

                // Validate the incoming data (dropoff location, etc.) as needed

                var request = new Request
                {
                    RequestId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                    PickupLocation = user.Location, // Assuming you have a Location property in the ApplicationUser model
                    DropoffLocation = createRequestVM.DropoffLocation,
                    EstimationPrice = Math.Round(DistanceService.EstimatePrice(user.Location, createRequestVM.DropoffLocation), 2),
                    PassengerId = passengerId,
                    PickupTime = DateTime.Now,
                    Status = Data.Enums.RequestStatus.Requested,
                    DriverID = createRequestVM.DriverId,
                };

                _requestRepository.Add(request);
                await _hubContext.Clients.User(request.DriverID).SendAsync("ReceiveRequestNotification", "New ride request!");

                _notyf.Success("Request Sent!");

                // You might want to return some information about the created request
                return RedirectToAction("PassengerDashboard", "Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ride request.");
                _notyf.Error("Error creating ride request.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        /// <summary>
        /// Endpoint to cancel a ride request.
        /// </summary>
        [HttpPost("cancelRequest")]
        public async Task<IActionResult> CancelRequest([FromBody] CancelRequestViewModel cancelRequestVM)
        {
            try
            {
                var request = await _requestRepository.GetByIdAsyncNoTracking(cancelRequestVM.RequestId);

                if (request != null)
                {
                    _requestRepository.Delete(request);
                    return Ok(new { Message = "Request cancelled successfully" });
                }

                return NotFound(new { Error = "Request not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling ride request.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        /// <summary>
        /// Endpoint to accept a ride request.
        /// </summary>
        [Route("api/request/acceptRequest")]
        [HttpPost("acceptRequest")]
        public async Task<IActionResult> AcceptRequest(string requestId, string passengerId)
        {
            try
            {
                var request = await _requestRepository.GetByIdAsyncNoTracking(requestId);
                var driverId = _httpContextAccessor.HttpContext.User.GetUserId();

                if (request != null)
                {
                    request.Status = Data.Enums.RequestStatus.Accepted;
                    _requestRepository.Update(request);

                    var trip = new Trip
                    {
                        TripId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                        DriverId = driverId,
                        RequestId = requestId,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow.AddMinutes(60),
                        Duration = 60,
                        Price = Math.Round(request.EstimationPrice, 2),
                        DriverRating = 5,
                        PassengerRating = 5,
                        Comment = "I like the driver",
                        PassengerId = passengerId,
                        DropoffLocation = request.DropoffLocation,
                        PickupLocation = request.PickupLocation,
                    };

                    _tripRepository.Add(trip);

                    _notyf.Success("Request Accepted Successfully");
                    await _hubContext.Clients.User(passengerId).SendAsync("ReceiveRequestNotification", "Your Ride Request Accepted!", driverId, trip.TripId);

                    return RedirectToAction("DriverDashboard", "Dashboard");
                }

                return NotFound(new { Error = "Request not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting ride request.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        /// <summary>
        /// Endpoint to get ride requests for a driver.
        /// </summary>
        [Route("api/request/getDriverRequests")]
        [HttpPost("getDriverRequests")]
        public async Task<IActionResult> GetDriverRequests(string driverId)
        {
            try
            {
                var requests = await _requestRepository.GetByUserId(driverId, Roles.Driver);
                List<GetRequestsViewModel> requestsList = new();

                foreach (var request in requests)
                {
                    var requestDetails = await _requestRepository.GetByIdAsyncNoTracking(request.RequestId);
                    var passenger = await _userRepository.GetByIdAsyncNoTracking(request.PassengerId);
                    requestsList.Add(new GetRequestsViewModel
                    {
                        PassengerName = passenger.FirstName + " " + passenger.LastName,
                        Status = requestDetails.Status,
                        NumberOfSeats = requestDetails.NumberOfSeats,
                        ApplicationUserId = requestDetails.PassengerId,
                        DropoffLocation = requestDetails.DropoffLocation,
                        PickupLocation = requestDetails.PickupLocation,
                        EstimationPrice = requestDetails.EstimationPrice,
                        PickupTime = requestDetails.PickupTime
                    });
                }

                return View(requestsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting driver requests.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        /// <summary>
        /// Endpoint to get ride requests for a passenger.
        /// </summary>
        [Route("api/request/getPassengerRequests")]
        [HttpPost("getPassengerRequests")]
        public async Task<IActionResult> GetPassengerRequests(string passengerId)
        {
            try
            {
                var requests = await _requestRepository.GetByUserId(passengerId, Roles.Passenger);
                List<GetRequestsViewModel> requestsList = new();

                foreach (var request in requests)
                {
                    var requestDetails = await _requestRepository.GetByIdAsyncNoTracking(request.RequestId);
                    var driver = await _userRepository.GetByIdAsyncNoTracking(request.PassengerId);
                    requestsList.Add(new GetRequestsViewModel
                    {
                        DriverName = driver.FirstName + " " + driver.LastName,
                        Status = requestDetails.Status,
                        NumberOfSeats = requestDetails.NumberOfSeats,
                        ApplicationUserId = requestDetails.PassengerId,
                        DropoffLocation = requestDetails.DropoffLocation,
                        PickupLocation = requestDetails.PickupLocation,
                        EstimationPrice = requestDetails.EstimationPrice,
                        PickupTime = requestDetails.PickupTime
                    });
                }

                return View(requestsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting passenger requests.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
