using Microsoft.AspNetCore.Mvc;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.ViewModel.Request;

namespace Yemeni_Driver.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestController(IRequestRepository requestRepository, IHttpContextAccessor httpContextAccessor)
        {
            _requestRepository = requestRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult CreateRequest(CreateRequestViewModel createRequestVM)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            _requestRepository.Add(new Models.Request
            {
                PickupLocation = createRequestVM.PickupLocation,
                DropoffLocation = createRequestVM.DropoffLocation,
                EstimationPrice = createRequestVM.EstimationPrice,
                ApplicationUserId = userId,
                NumberOfSeats = createRequestVM.NumberOfSeats,
                PickupTime = createRequestVM.PickupTime,
                Status = createRequestVM.Status,
                RequestId = createRequestVM.RequestId,
            });
            

            return View();
        }
    }
}
