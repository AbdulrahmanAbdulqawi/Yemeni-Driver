using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Data.Enums;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.ViewModel.Rating;

namespace YemeniDriver.Controllers
{
    /// <summary>
    /// Controller responsible for handling driver ratings and reviews.
    /// </summary>
    public class RatingController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IDriverRatingReposiotry _driverRatingReposiotry;
        private readonly IRequestRepository _requestRepository;

        // Constructor with dependency injection
        public RatingController(
            IUserRepository userRepository,
            IVehicleRepository vehicleRepository,
            ITripRepository tripRepository,
            IDriverRatingReposiotry driverRatingReposiotry,
            IRequestRepository requestRepository)
        {
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _tripRepository = tripRepository;
            _driverRatingReposiotry = driverRatingReposiotry;
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Display the index page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display the rating and review input box.
        /// </summary>
        /// <param name="driverId">ID of the driver.</param>
        /// <param name="tripId">ID of the trip.</param>
        public async Task<IActionResult> ShowRatingAndReviewBox(string driverId, string tripId)
        {
            // Retrieve driver, vehicle, and trip details
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            var trip = await _tripRepository.GetByIdAsyncNoTracking(tripId);
            driver.Vehicle = vehicle;

            // Create the ViewModel for rating and review
            var rateVm = new ShowRatingAndReviewViewModel
            {
                Driver = driver,
                Trip = trip,
                DriverID = driverId,
                TripId = tripId,
            };

            return View(rateVm);
        }

        /// <summary>
        /// Handle the submission of the rating and review form.
        /// </summary>
        /// <param name="rateVM">ViewModel containing rating and review details.</param>
        [HttpPost]
        public async Task<IActionResult> ShowRatingAndReviewBox(ShowRatingAndReviewViewModel rateVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Retrieve trip, request, and driver details
                    var trip = await _tripRepository.GetByIdAsyncNoTracking(rateVM.TripId);
                    var request = await _requestRepository.GetByIdAsyncNoTracking(trip.RequestId);
                    var driver = await _userRepository.GetByIdAsyncNoTracking(trip.DriverId);

                    // Save the rating and perform necessary actions
                    _driverRatingReposiotry.Add(new DriverRating
                    {
                        RatingValue = (int)rateVM.RatingValue,
                        DriverId = rateVM.DriverID,
                        TripId = rateVM.TripId,
                        Comment = rateVM.Comment,
                    });
                    _driverRatingReposiotry.Save();

                    // Update trip, request, and driver information
                    UpdateTrip(trip, (int)rateVM.RatingValue, rateVM?.Comment);
                    UpdateRequest(request);
                    driver.Rating = await CalcDriverRatingsAverage(driver.Id);
                    _userRepository.Update(driver);
                    _userRepository.Save();

                    // Redirect to a thank-you page or any other desired page
                    return RedirectToAction("PassengerDashboard", "Dashboard");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                ModelState.AddModelError("", "An error occurred while processing your request.");
            }

            // If there are validation errors or other issues, return the view with the ViewModel
            return View(rateVM);
        }

        /// <summary>
        /// Calculate the average rating of a driver.
        /// </summary>
        /// <param name="driverId">ID of the driver.</param>
        private async Task<int> CalcDriverRatingsAverage(string driverId)
        {
            var driverRatings = await _driverRatingReposiotry.GetRatingsByDriverId(driverId);
            var ratingsAvg = (int)driverRatings.Select(a => a.RatingValue).Average();
            return ratingsAvg;
        }

        /// <summary>
        /// Update the request status to completed.
        /// </summary>
        /// <param name="request">Request to be updated.</param>
        private void UpdateRequest(Request request)
        {
            request.Status = RequestStatus.Completed;
            _requestRepository.Update(request);
            _requestRepository.Save();
        }

        /// <summary>
        /// Update the trip information with rating and comment.
        /// </summary>
        /// <param name="trip">Trip to be updated.</param>
        /// <param name="ratingValue">Rating value.</param>
        /// <param name="comment">Review comment.</param>
        private void UpdateTrip(Trip trip, int ratingValue, string comment)
        {
            trip.DriverRating = ratingValue;
            trip.Comment = comment;
            _tripRepository.Update(trip);
            _tripRepository.Save();
        }
    }
}
