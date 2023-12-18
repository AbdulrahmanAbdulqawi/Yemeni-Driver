using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.ViewModel.Vehicle;

namespace YemeniDriver.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public VehicleController(IVehicleRepository vehicleRepository, IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> UpdateVehicle()
        
        {
            // Retrieve a list of drivers to populate a dropdown in the form
            var drivers =  _userRepository.GetDrivers().Result.Select(a => new SelectListItem { Text = a.FirstName, Value = a.Id }).ToList();
            ViewBag.Drivers = drivers;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVehicle(UpdateVehicleViewModel updateVehicleVM)
        {
            if(ModelState.IsValid) {

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
            return View(TempData["Error"] = "Create a new vehicle failed!");
        }
    }
}
