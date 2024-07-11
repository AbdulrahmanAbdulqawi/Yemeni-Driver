using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this for ILogger
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class LocationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LocationController> _logger;
        private readonly IGeocodingService _geocodingService; // Use an interface for GeocodingService

        public LocationController(IUserRepository userRepository, ILogger<LocationController> logger, IGeocodingService geocodingService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _geocodingService = geocodingService;
        }

        [HttpPost("updateLiveLocation")]
        public async Task<ActionResult> UpdateLiveLocationAsync([FromBody] LiveLocationModel liveLocation)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return BadRequest("User not authenticated.");
                }

                var user = await _userRepository.GetByIdAsyncNoTracking(userId);
                user.LiveLocationLatitude = liveLocation.Latitude;
                user.LiveLocationLongitude = liveLocation.Longitude;

                var location = await _geocodingService.GetAddressFromCoordinates(liveLocation.Latitude, liveLocation.Longitude);

                user.Location = $"{location.Country}, {location.City}, {location.Address}, {location.Zip}";

                _userRepository.Update(user);

                return Ok("location updated successfully." );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateLiveLocationAsync method for User ID: {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}");
                return StatusCode(500, "An error occurred while updating live location.");
            }
        }
    }

    public class LiveLocationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
