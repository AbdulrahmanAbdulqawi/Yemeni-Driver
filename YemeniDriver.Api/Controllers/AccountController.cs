using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.ViewModel.Account;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace YemeniDriver.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly INotyfService _notyf;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IPhotoService photoService,
            IVehicleRepository vehicleRepository,
            INotyfService notyf)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _notyf = notyf;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterationViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(registerVM.Email);

                if (userExists == null)
                {
                    var appUser = new ApplicationUser { UserName = registerVM.Email, Email = registerVM.Email };
                    var result = await _userManager.CreateAsync(appUser, registerVM.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(appUser, isPersistent: false);
                        return Ok(new { Message = "Registration successful" });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    return BadRequest(new { Message = "User already exists" });
                }
            }

            return BadRequest(new { Message = "Registration failed" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return BadRequest(new { Message = "Invalid model state" });

            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            var role = await DetermineUserRole(user);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginVM.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    _notyf.Success($"{user.FirstName} is logged in");
                    return Ok("Login successfully");
                }
            }

            _notyf.Error("Wrong credentials. Please enter the correct credentials");
            return BadRequest(new { Message = "Login failed" });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }

        private async Task<string> DetermineUserRole(ApplicationUser user)
        {
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.Driver.ToString())) return Roles.Driver.ToString();
            if (roles.Contains(Roles.Passenger.ToString())) return Roles.Passenger.ToString();

            return Roles.Admin.ToString();
        }
    }
}
