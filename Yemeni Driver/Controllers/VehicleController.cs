using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;
using Yemeni_Driver.ViewModel.Vehicle;

namespace Yemeni_Driver.Controllers
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
        public async Task<IActionResult> CreateVehicle()
        
        {
            // Retrieve a list of drivers to populate a dropdown in the form
            var drivers =  _userRepository.GetDrivers().Result.Select(a => new SelectListItem { Text = a.FirstName, Value = a.Id }).ToList();
            ViewBag.Drivers = drivers;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(CreateVehicleViewModel createVehicleVM)
        {
            if(ModelState.IsValid) {

                var vehicle = await _vehicleRepository.GetVehicleByIdAsync(createVehicleVM.UserId);
                // Map the ViewModel to the Entity
                var newVehicle = new Vehicle
                {
                    ApplicationUserId = createVehicleVM.UserId,
                    VehicleId = vehicle.VehicleId,
                    Model = createVehicleVM.Model,
                    Make = createVehicleVM.Make,
                    Year = createVehicleVM.Year,
                    Capacity = createVehicleVM.Capacity,
                    Color = createVehicleVM.Color,
                    PlateNumber = createVehicleVM.PlateNumber,
                };
                _vehicleRepository.Update(newVehicle);

                return RedirectToAction("Index", "Home");
            }
            return View(TempData["Error"] = "Create a new vehicle failed!");
        }
    }
}
