using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.ViewModel.Trip;

namespace YemeniDriver.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TripController : ControllerBase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TripController> _logger;

        public TripController(ITripRepository tripRepository, IUserRepository userRepository, ILogger<TripController> logger)
        {
            _tripRepository = tripRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("DriverTrips/{driverId}")]
        [Authorize(Roles = "Driver")]
        public async Task<ActionResult<IEnumerable<GetTripsViewModel>>> GetDriverTrips(string driverId)
        {
            return await GetTrips(driverId, Roles.Driver);
        }

        [HttpGet("PassengerTrips/{passengerId}")]
        [Authorize(Roles = "Passenger")]
        public async Task<ActionResult<IEnumerable<GetTripsViewModel>>> GetPassengerTrips(string passengerId)
        {
            return await GetTrips(passengerId, Roles.Passenger);
        }



        private async Task<ActionResult<IEnumerable<GetTripsViewModel>>> GetTrips(string userId, Roles role)
        {
            try
            {
                var trips = await _tripRepository.GetByUserId(userId, role);
                var tripsVM = new List<GetTripsViewModel>();

                foreach (var trip in trips)
                {
                    var otherUserId = role == Roles.Driver ? trip.PassengerId : trip.DriverId;
                    var otherUser = role == Roles.Driver
                        ? await _userRepository.GetPassengerById(otherUserId)
                        : await _userRepository.GetDriverById(otherUserId);

                    var tripVM = MapToTripsViewModel(trip, otherUser, role);
                    tripsVM.Add(tripVM);
                }

                return Ok(tripsVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting {role.ToString().ToLower()} trips.");
                return Problem("Internal Server Error", statusCode: 500);
            }
        }

        private GetTripsViewModel MapToTripsViewModel(Trip trip, ApplicationUser otherUser, Roles role)
        {
            // Map data to view model based on role
            return new GetTripsViewModel
            {
                PassengerName = role == Roles.Driver ? $"{otherUser.FirstName} {otherUser.LastName}" : $"{otherUser.FirstName} {otherUser.LastName}",
                StartTime = trip.StartTime,
                EndTime = trip.EndTime,
                ApplicationUserId = otherUser.Id,
                Comment = trip.Comment,
                DriverRating = trip.DriverRating,
                Duration = trip.Duration,
                PassengerRating = trip.PassengerRating,
                Price = Math.Round(trip.Price, 2),
                RequestId = trip.RequestId,
                DropoffLocation = trip.DropoffLocation,
                PickupLocation = trip.PickupLocation,
            };
        }
    }
}
