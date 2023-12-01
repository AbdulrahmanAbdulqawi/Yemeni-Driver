using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;
using Yemeni_Driver.Service;
using Yemeni_Driver.ViewModel.Request;

namespace Yemeni_Driver.Controllers
{
    [ApiController]
    [Route("api/request")]
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

        [HttpGet("createRequest")]
        public IActionResult CreateRequest()
        {
            return RedirectToAction("PassengerDashboard", "Dashboard");
        }

        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequest(CreateRequestViewModel createRequestVM)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            _requestRepository.Add(new Request
            {
                RequestId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                PickupLocation = user.Result.Location,
                DropoffLocation = createRequestVM.DropoffLocation,
                EstimationPrice = DistanceService.EstimatePrice( user.Result.Location, createRequestVM.DropoffLocation),
                ApplicationUserId = userId,
                PickupTime = DateTime.Now,
                Status = Data.Enums.RequestStatus.Requested,
            });

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New ride request available!");

            return RedirectToAction("PassengerDashboard", "Dashboard"); // Redirect to the home page after successful registration

        }

        [HttpPost("cancelRequest")]
        public async Task<IActionResult> CancelRequest(string requestId)
        {
            var request = await _requestRepository.GetByIdAsyncNoTracking(requestId);
            if (request != null)
            {
                _requestRepository.Delete(request);
                return RedirectToAction("PassengerDashboard", "Dashboard");
            }
            return (IActionResult)(TempData["Error"] = "Request is not found");
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
