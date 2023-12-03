﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;
using Yemeni_Driver.Service;
using Yemeni_Driver.ViewModel.Request;

namespace Yemeni_Driver.Controllers
{
    [ApiController]
    [Route("api/request/")]
    public class RequestController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDriverAndRequestRepository _driverAndRequestRepository;
        public RequestController(IRequestRepository requestRepository, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager, IDriverAndRequestRepository driverAndRequestRepository)
        {
            _requestRepository = requestRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _userManager = userManager;
            _driverAndRequestRepository = driverAndRequestRepository;
        }
        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestViewModel createRequestVM)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if(_requestRepository.GetAll().Result.Any(a => a.ApplicationUserId == userId && a.Status == Data.Enums.RequestStatus.Requested))
            {
                return View(TempData["Error"] == "Request is in progress");
            }
            // Validate the incoming data (dropoff location, etc.) as needed

            var request = new Request
            {
                RequestId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                PickupLocation = user.Location, // Assuming you have a Location property in the ApplicationUser model
                DropoffLocation = createRequestVM.DropoffLocation,
                EstimationPrice = DistanceService.EstimatePrice(user.Location, createRequestVM.DropoffLocation),
                ApplicationUserId = userId,
                PickupTime = DateTime.Now,
                Status = Data.Enums.RequestStatus.Requested,
            };

            _requestRepository.Add(request);

            //await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New ride request available!");

            // You might want to return some information about the created request
            return RedirectToAction("PassengerDashboard", "Dashboard");
        }

        [HttpPost("cancelRequest")]
        public async Task<IActionResult> CancelRequest([FromBody] CancelRequestViewModel cancelRequestVM)
        {
            var request = await _requestRepository.GetByIdAsyncNoTracking(cancelRequestVM.RequestId);
            if (request != null)
            {
                _requestRepository.Delete(request);
                return Ok(new { Message = "Request cancelled successfully" });
            }
            return NotFound(new { Error = "Request not found" });
        }
        [Route("api/request")]
        [HttpPost("acceptRequest")]
        public async Task<IActionResult> AcceptRequest(string requestId)
        {
            var request = await _requestRepository.GetByIdAsyncNoTracking(requestId);
            var driverId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (request != null)
            {
                request.Status = Data.Enums.RequestStatus.Accepted;
                _requestRepository.Update(request);
                var requestAndDriver = new DriverAndRequest
                {
                    ApplicationUserId = driverId,
                    RequestId = requestId
                };
                _driverAndRequestRepository.Add(requestAndDriver);
                return RedirectToAction("DriverDashboard", "Dashboard");
            }
            return NotFound(new { Error = "Request not found" });
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
