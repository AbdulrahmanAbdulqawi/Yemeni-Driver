using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.Service;
using YemeniDriver.Api.ViewModel.Request;

namespace YemeniDriver.Api.Controllers
{
    [ApiController]
    [Route("api/request/")]
    [Authorize]
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

        [HttpPost("createRequest")]
        [Authorize(Roles = "Passenger")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestViewModel createRequestVM)
        {
            try
            {
                var passengerId = _httpContextAccessor.HttpContext.User.GetUserId();
                var user = await _userRepository.GetPassengerById(passengerId);

                if (user == null)
                {
                    return NotFound(new { Message = "Passenger not found" });
                }

                var existingRequests = await _requestRepository.GetByUserId(passengerId, Roles.Passenger);

                if (existingRequests?.Any(a => a.Status == Data.Enums.RequestStatus.Requested) == true)
                {
                    _notyf.Information("Request In Progress");
                    return Conflict(new { Message = "Request in progress" });
                }

                var request = new Request
                {
                    RequestId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                    PickupLocation = user.Location ?? "DefaultLocation",
                    DropoffLocation = createRequestVM.DropoffLocation,
                    EstimationPrice = Math.Round(DistanceService.EstimatePrice(user.Location ?? "DefaultLocation", createRequestVM.DropoffLocation), 2),
                    PassengerId = passengerId,
                    PickupTime = DateTime.Now,
                    Status = Data.Enums.RequestStatus.Requested,
                    DriverID = createRequestVM.DriverId,
                };

                _requestRepository.Add(request);
                await _hubContext.Clients.User(request.DriverID).SendAsync("ReceiveRequestNotification", "New ride request!");

                _notyf.Success("Request Sent!");

                return CreatedAtAction(nameof(CreateRequest), new { requestId = request.RequestId }, new { Message = "Request Created" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ride request.");
                _notyf.Error("Error creating ride request.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

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


        [HttpPost("acceptRequest")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> AcceptRequest([FromBody] AcceptRequestViewModel acceptRequestVM)
        {
            try
            {
                var request = await _requestRepository.GetByIdAsyncNoTracking(acceptRequestVM.RequestId);
                var driverId = _httpContextAccessor.HttpContext.User.GetUserId();

                if (request != null)
                {
                    request.Status = RequestStatus.Accepted;
                    _requestRepository.Update(request);

                    var trip = new Trip
                    {
                        TripId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                        DriverId = driverId,
                        RequestId = acceptRequestVM.RequestId,
                        StartTime = DateTime.UtcNow,
                        EndTime = DateTime.UtcNow.AddMinutes(60),
                        Duration = 60,
                        Price = Math.Round(request.EstimationPrice, 2),
                        DriverRating = 5,
                        PassengerRating = 5,
                        Comment = "I like the driver",
                        PassengerId = request.PassengerId,
                        DropoffLocation = request.DropoffLocation,
                        PickupLocation = request.PickupLocation,
                    };

                    _tripRepository.Add(trip);

                    _notyf.Success("Request Accepted Successfully");
                    await _hubContext.Clients.User(request.PassengerId).SendAsync("ReceiveRequestNotification", "Your Ride Request Accepted!", driverId, trip.TripId);

                    return Ok("Request Accepted");
                }

                return NotFound(new { Error = "Request not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting ride request.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        [HttpPost("getDriverRequests")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> GetDriverRequests()
        {
            try
            {
                var driverId = _httpContextAccessor.HttpContext.User.GetUserId();
                var requests = await _requestRepository.GetByUserId(driverId, Roles.Driver);

                if (requests == null || !requests.Any())
                {
                    return NotFound("No requests exist");
                }

                var requestsList = new List<GetRequestsViewModel>();

                foreach (var request in requests)
                {
                    var requestDetails = await _requestRepository.GetByIdAsyncNoTracking(request.RequestId);
                    var passenger = await _userRepository.GetPassengerById(request.PassengerId);

                    requestsList.Add(new GetRequestsViewModel
                    {
                        PassengerName = $"{passenger.FirstName} {passenger.LastName}",
                        Status = requestDetails.Status,
                        NumberOfSeats = requestDetails.NumberOfSeats,
                        ApplicationUserId = requestDetails.PassengerId,
                        DropoffLocation = requestDetails.DropoffLocation,
                        PickupLocation = requestDetails.PickupLocation,
                        EstimationPrice = requestDetails.EstimationPrice,
                        PickupTime = requestDetails.PickupTime
                    });
                }

                return Ok(requestsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting driver requests.");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        [HttpPost("getPassengerRequests")]
        [Authorize(Roles = "Passenger")]
        public async Task<IActionResult> GetPassengerRequests()
        {
            try
            {
                var passengerId = _httpContextAccessor.HttpContext.User.GetUserId();
                var requests = await _requestRepository.GetByUserId(passengerId, Roles.Passenger);

                if (requests == null || !requests.Any())
                {
                    return NotFound("No requests exist");
                }

                var requestsList = new List<GetRequestsViewModel>();

                foreach (var request in requests)
                {
                    var requestDetails = await _requestRepository.GetByIdAsyncNoTracking(request.RequestId);
                    var driver = await _userRepository.GetDriverById(request.DriverID);

                    requestsList.Add(new GetRequestsViewModel
                    {
                        DriverName = $"{driver.FirstName} {driver.LastName}",
                        Status = requestDetails.Status,
                        NumberOfSeats = requestDetails.NumberOfSeats,
                        ApplicationUserId = requestDetails.DriverID,
                        DropoffLocation = requestDetails.DropoffLocation,
                        PickupLocation = requestDetails.PickupLocation,
                        EstimationPrice = requestDetails.EstimationPrice,
                        PickupTime = requestDetails.PickupTime
                    });
                }

                return Ok(requestsList);
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
