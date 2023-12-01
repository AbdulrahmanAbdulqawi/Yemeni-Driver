using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Service;
using Yemeni_Driver.ViewModel.Request;

namespace Yemeni_Driver.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContext;

        public RequestController(IRequestRepository requestRepository, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext)
        {
            _requestRepository = requestRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public IActionResult CreateRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestViewModel createRequestVM)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            _requestRepository.Add(new Models.Request
            {
                RequestId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                PickupLocation = createRequestVM.PickupLocation,
                DropoffLocation = createRequestVM.DropoffLocation,
                EstimationPrice = createRequestVM.EstimationPrice,
                ApplicationUserId = userId,
                NumberOfSeats = createRequestVM.NumberOfSeats,
                PickupTime = createRequestVM.PickupTime,
                Status = createRequestVM.Status,
            });

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "New ride request available!");

            return Ok();

        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
