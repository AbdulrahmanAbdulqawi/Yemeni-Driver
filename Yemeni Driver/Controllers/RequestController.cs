using Microsoft.AspNetCore.Identity;
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

        public RequestController(IRequestRepository requestRepository, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _requestRepository = requestRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _userManager = userManager;
        }
        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestViewModel createRequestVM)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

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

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
