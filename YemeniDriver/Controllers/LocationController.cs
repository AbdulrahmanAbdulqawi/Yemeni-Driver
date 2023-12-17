using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Service;

namespace YemeniDriver.Controllers
{
    [ApiController]
    [Route("api")]
    public class LocationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("updateLiveLocation")]
        public async Task<IActionResult> UpdateLiveLocation([FromBody] LiveLocationModel liveLocation)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("User not authenticated.");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            user.LiveLocationLatitude = liveLocation.Latitude;
            user.LiveLocationLongitude = liveLocation.Longitude;

            var googleMapService = new GeocodingService(Constants.API_KEY);

            var location = await googleMapService.GetAddressFromCoordinates(liveLocation.Latitude, liveLocation.Longitude);

            user.Location = location.Country + ", " + location.City + ", " + location.Address + ", " + location.Zip;

            _userRepository.Update(user);
          
            return Ok(new { Message = "Live location updated successfully." });
        }
    }


    public class LiveLocationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
