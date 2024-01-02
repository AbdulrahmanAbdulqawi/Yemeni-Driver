using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Service;

namespace YemeniDriver.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling location-related API requests.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class LocationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LocationController> _logger; // Add ILogger

        // Constructor with dependency injection
        public LocationController(IUserRepository userRepository, ILogger<LocationController> logger) // Inject ILogger in the constructor
        {
            _userRepository = userRepository;
            _logger = logger; // Assign ILogger in the constructor
        }

        /// <summary>
        /// Updates the live location of the user.
        /// </summary>
        /// <param name="liveLocation">The live location coordinates.</param>
        [HttpPost("updateLiveLocation")]
        public async Task<IActionResult> UpdateLiveLocation([FromBody] LiveLocationModel liveLocation)
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

                var googleMapService = new GeocodingService(Data.Constants.API_KEY);

                var location = await googleMapService.GetAddressFromCoordinates(liveLocation.Latitude, liveLocation.Longitude);

                user.Location = location.Country + ", " + location.City + ", " + location.Address + ", " + location.Zip;

                _userRepository.Update(user);

                return Ok(new { Message = "Live location updated successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error in UpdateLiveLocation method.");
                // Optionally handle the exception and return an error response or redirect
                return StatusCode(500, "An error occurred while updating live location.");
            }
        }
    }

    /// <summary>
    /// Model representing the live location coordinates.
    /// </summary>
    public class LiveLocationModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
