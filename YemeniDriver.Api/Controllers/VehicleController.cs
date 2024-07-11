using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.ViewModel.Vehicle;

namespace YemeniDriver.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling vehicle-related actions and views.
    /// </summary>
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<VehicleController> _logger;

        /// <summary>
        /// Constructor for the VehicleController.
        /// </summary>
        public VehicleController(IVehicleRepository vehicleRepository, IUserRepository userRepository, UserManager<ApplicationUser> userManager, ILogger<VehicleController> logger)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Action to display the form for updating a vehicle.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UpdateVehicle()
        {
            try
            {
                // Retrieve a list of drivers to populate a dropdown in the form
                var drivers = await _userRepository.GetDrivers();
                var driverSelectList = drivers.Select(a => new SelectListItem { Text = a.FirstName, Value = a.Id }).ToList();
                ViewBag.Drivers = driverSelectList;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving drivers for vehicle update form.");
                return View("Error");
            }
        }

        /// <summary>
        /// Action to handle the submission of the vehicle update form.
        /// </summary>
        [HttpPost("updateVehicle")]
        public async Task<IActionResult> UpdateVehicle(UpdateVehicleViewModel updateVehicleVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vehicle = await _vehicleRepository.GetVehicleByIdAsync(updateVehicleVM.UserId);

                    // Map the ViewModel to the Entity
                    var newVehicle = new Vehicle
                    {
                        DriverId = updateVehicleVM.UserId,
                        VehicleId = vehicle.VehicleId,
                        Model = updateVehicleVM.Model,
                        Make = updateVehicleVM.Make,
                        Year = updateVehicleVM.Year,
                        Capacity = updateVehicleVM.Capacity,
                        Color = updateVehicleVM.Color,
                        PlateNumber = updateVehicleVM.PlateNumber,
                        VehiclImageUrl = updateVehicleVM.VehicleImageUrl,
                    };

                    _vehicleRepository.Update(newVehicle);

                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Create a new vehicle failed!";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating the vehicle.");
                return View("Error");
            }
        }
    }
}
