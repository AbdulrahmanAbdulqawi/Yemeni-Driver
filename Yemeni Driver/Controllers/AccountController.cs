using CloudinaryDotNet.Actions;
using GoogleMapsApi.Entities.Directions.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;
using Yemeni_Driver.ViewModel.Account;
using static System.Net.Mime.MediaTypeNames;

namespace Yemeni_Driver.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;



        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IPhotoService photoService, IVehicleRepository vehicleRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _photoService = photoService;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
        }

        public IActionResult Register()
        {
            var response = new RegisterationViewModel();
            return View(response);
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(registerVM.Email);
                if(userExists == null)
                {
                    var appUser = new ApplicationUser { UserName = registerVM.Email, Email = registerVM.Email };
                    var result = await _userManager.CreateAsync(appUser, registerVM.Password);

                    if (result.Succeeded)
                    {
                      
                        //await _userManager.AddToRoleAsync(appUser, Roles.User);
                        
                        await _signInManager.SignInAsync(appUser, isPersistent: false);
                        return RedirectToAction("Index", "Home"); // Redirect to the home page after successful registration
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    TempData["Error"] = "User already exist";
                    return View(registerVM);
                }
                
            }
            return (IActionResult)(TempData["Error"] = "Registeration Failed");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            var drivers = await _userManager.GetUsersInRoleAsync(Roles.Driver.ToString());
            var passengers = await _userManager.GetUsersInRoleAsync(Roles.Passenger.ToString());
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        if(drivers.Contains(user))
                        {
                            return RedirectToAction("DriverDashboard", "Dashboard");
                        }else if(passengers.Contains(user))
                        {
                            return RedirectToAction("PassengerDashboard", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("AdminDashboard", "Dashboard");
                        }
                    }
                }
                //password doesnt match 
                TempData["Error"] = "Wrong credentials. Please enter the correct credentials";
                return View(loginVM);
            }
            TempData["Error"] = "Wrong credentials. Please enter the correct credentials";
            return View(loginVM);
        }

        public IActionResult RegisterAsDriver()
        {
            var response = new DriverRegisterationViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsDriver(DriverRegisterationViewModel driverRegisterVM)
        {
            var getDriver = await _userManager.FindByEmailAsync(driverRegisterVM.Email);

            if (getDriver != null) {
                TempData["Error"] = "User Already Exists!";
                return View(driverRegisterVM);
            }
            var profileImageUploadResult = await _photoService.AddPhotoAsync(driverRegisterVM.ProfileImage);


            var appUser = new ApplicationUser { UserName = driverRegisterVM.Email, Email = driverRegisterVM.Email,
                DrivingLicenseNumber = driverRegisterVM.DrivingLicenseNumber,
                FirstName = driverRegisterVM.FirstName, LastName = driverRegisterVM.LastName, Gender = driverRegisterVM.Gender, PhoneNumber = driverRegisterVM.PhoneNumber,
                VehicleId = driverRegisterVM.VehicleId,
                ProfileImageUrl = profileImageUploadResult.Url.ToString(),
            };


            var vehicleImageUploadResult = await _photoService.AddPhotoAsync(driverRegisterVM.ProfileImage);

            var newVehicle = new Models.Vehicle
            {
                ApplicationUserId = appUser.Id,
                VehicleId = appUser.VehicleId,
                Capacity = driverRegisterVM.Capacity,
                Color = driverRegisterVM.Color,
                Model = driverRegisterVM.Model,
                Year = driverRegisterVM.Year,
                Make = driverRegisterVM.Make,
                PlateNumber = driverRegisterVM.PlateNumber,
                VehiclImageUrl = vehicleImageUploadResult.Url.ToString()
            };

            appUser.Vehicle = newVehicle;

            var result = await _userManager.CreateAsync(appUser, driverRegisterVM.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, Roles.Driver.ToString());
                await _signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("DriverDashboard", "Dashboard"); // Redirect to the home page after successful registration
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return (IActionResult)(TempData["Error"] = "Registeration Failed");

            //test


        }

        public IActionResult RegisterAsPassenger()
        {
            var response = new PassengerRegisterationViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsPassenger(PassengerRegisterationViewModel registerVM)
        {
            var getDriver = await _userManager.FindByEmailAsync(registerVM.Email);

            if (getDriver != null)
            {
                TempData["Error"] = "User Already Exists!";
                return View(registerVM);
            }

            if(!ModelState.IsValid)
            {
                TempData["Error"] = "entries are invalid!";
                return View(registerVM);
            }

            var profileImageUploadResult = await _photoService.AddPhotoAsync(registerVM.ProfileImage);

            var appUser = new ApplicationUser
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Gender = registerVM.Gender,
                PhoneNumber = registerVM.PhoneNumber,
                ProfileImageUrl = profileImageUploadResult.Url.ToString()
            };
            var result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(appUser, Roles.Passenger.ToString());

                await _signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("PassengerDashboard", "Dashboard"); // Redirect to the home page after successful registration
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return (IActionResult)(TempData["Error"] = "Registeration Failed");

        }

        public async Task<IActionResult> ViewDriverDetails(string driverId)
        {
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            if (driver == null)
            {
                return NotFound(); // Handle the case where the driver is not found
            }


            DriverDetailsViewModel driverVM = new DriverDetailsViewModel()
            {
                Email = driver.Email,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Image = driver.ProfileImageUrl,
                Location = driver.Location,
                PhoneNumber = driver.PhoneNumber,
                Rating = 5,
                ViewVehicle = new ViewModel.Vehicle.ViewVehicleViewModel
                {
                    Capacity = vehicle.Capacity,
                    Color = vehicle.Color,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    PlateNumber = vehicle.PlateNumber,
                    VehiclImageUrl = vehicle.VehiclImageUrl,
                    Year = vehicle.Year,
                }
            };

            return View(driverVM);
        }

        public async Task<IActionResult> EditDriverDetails(string driverId)
        {
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            
            if(driver == null)
            {
                return NotFound(driverId);
            }
            var editDriverDetailesVM = new EditDriverDetailsViewModel()
            {
                DrivingLicenseNumber = driver.DrivingLicenseNumber,
                Email = driver.Email,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Gender = (Data.Enums.Gender)driver.Gender,
                ProfileImageUrl = driver.ProfileImageUrl,
                PhoneNumber = driver.PhoneNumber,
                
                Vehicle = new Models.Vehicle
                {
                    VehicleId = vehicle.VehicleId,
                    ApplicationUserId = vehicle.ApplicationUserId,
                    Capacity = vehicle.Capacity,
                    Color = vehicle.Color,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    PlateNumber = vehicle.PlateNumber,
                    VehiclImageUrl = vehicle.VehiclImageUrl,
                    Year = vehicle.Year,
                }
            };

            return View(editDriverDetailesVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditDriverDetails(string driverId, EditDriverDetailsViewModel editDriverDetailsViewModel)
        {
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            ModelState.Remove("Vehicle.ApplicationUser");
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to update driver");
                return View("Error", editDriverDetailsViewModel);
            }
            try
            {
                await _photoService.DeletePhotoAsync(driver.ProfileImageUrl);
                if(vehicle.VehiclImageUrl != null)
                {
                    await _photoService.DeletePhotoAsync(vehicle.VehiclImageUrl);
                }
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "couldnt delete photo");
                return View(editDriverDetailsViewModel);
            }

            try
            {
                var profileImageResult = await _photoService.AddPhotoAsync(editDriverDetailsViewModel.ProfileImage);
                var vehicleImageResult = await _photoService.AddPhotoAsync(editDriverDetailsViewModel.VehicleImage);



                driver.FirstName = editDriverDetailsViewModel.FirstName;
                driver.LastName = editDriverDetailsViewModel.LastName;
                driver.Email = editDriverDetailsViewModel.Email;
                driver.Gender = editDriverDetailsViewModel.Gender;
                driver.PhoneNumber = editDriverDetailsViewModel.PhoneNumber;
                driver.ProfileImageUrl = profileImageResult.Url.ToString();
                _userRepository.Update(driver);


                var vehicletoUpdate = editDriverDetailsViewModel.Vehicle;
                vehicletoUpdate.ApplicationUserId = driverId;
                vehicletoUpdate.VehicleId = vehicle.VehicleId;
                vehicletoUpdate.VehiclImageUrl = vehicleImageResult.Url.ToString();
                _vehicleRepository.Update(vehicletoUpdate);
                
                return RedirectToAction("DriverDashboard", "Dashboard");
            }
            catch (Exception)
            {

                throw;
            }
           

        }




    }
}
