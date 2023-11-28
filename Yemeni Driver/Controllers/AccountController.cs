using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;
using Yemeni_Driver.ViewModel.Account;
using Yemeni_Driver.ViewModel.Home;

namespace Yemeni_Driver.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _UserRepository;
        private readonly ApplicationDbContext _applicationDb;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext applicationDb, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDb = applicationDb;
            _httpContextAccessor = httpContextAccessor;
            _UserRepository = userRepository;
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
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
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
            var response = new RegisterationViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsDriver(RegisterationViewModel driverRegisterVM)
        {
            var getDriver = await _userManager.FindByEmailAsync(driverRegisterVM.Email);

            if (getDriver != null) {
                TempData["Error"] = "User Already Exists!";
                return View(driverRegisterVM);
            }

            var appUser = new ApplicationUser { UserName = driverRegisterVM.Email, Email = driverRegisterVM.Email, 
                 DrivingLicenseNumber = driverRegisterVM.DrivingLicenseNumber,
                FirstName = driverRegisterVM.FirstName, LastName = driverRegisterVM.LastName, Gender = driverRegisterVM.Gender, PhoneNumber = driverRegisterVM.PhoneNumber,
                VehicleId = driverRegisterVM.VehicleId
               
            };

            var newVehicle = new Vehicle
            {
                ApplicationUserId = appUser.Id,
                VehicleId = appUser.VehicleId,
                Capacity = driverRegisterVM.Capacity,
                Color = driverRegisterVM.Color,
                Model = driverRegisterVM.Model,
                Year = driverRegisterVM.Year,
                Make = driverRegisterVM.Make,
                PlateNumber = driverRegisterVM.PlateNumber,
            };

            appUser.Vehicle = newVehicle;

            var result = await _userManager.CreateAsync(appUser, driverRegisterVM.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, Roles.Driver.ToString());
                await _signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("Index", "Home"); // Redirect to the home page after successful registration
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
            var response = new RegisterationViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsPassenger(RegisterationViewModel registerVM)
        {
            var getDriver = await _userManager.FindByEmailAsync(registerVM.Email);

            if (getDriver != null)
            {
                TempData["Error"] = "User Already Exists!";
                return View(registerVM);
            }

            var appUser = new ApplicationUser
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                DrivingLicenseNumber = registerVM.DrivingLicenseNumber,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Gender = registerVM.Gender,
                PhoneNumber = registerVM.PhoneNumber,

            };
            var result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(appUser, Roles.Passenger.ToString());

                await _signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("Index", "Home"); // Redirect to the home page after successful registration
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return (IActionResult)(TempData["Error"] = "Registeration Failed");

            //test


        }



    }
}
