using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.ViewModel.Account;

namespace YemeniDriver.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpPost("register")]
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

        [HttpPost("login")]
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
                    return Ok("Login successfully");
                }
            }

            return BadRequest(new { Message = "Login failed" });
        }

        [HttpPost("logout")]
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
