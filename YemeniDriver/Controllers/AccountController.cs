using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.ViewModel.Account;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace YemeniDriver.Controllers
{/// <summary>
 /// Controller for handling user account-related actions, including registration, login, and role-specific registration.
 /// </summary>
    public class AccountController : Controller
    {
        // User management and sign-in services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Services for photo upload and notification
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly INotyfService _notyf;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">User management service.</param>
        /// <param name="signInManager">Sign-in management service.</param>
        /// <param name="userRepository">User repository.</param>
        /// <param name="photoService">Photo upload service.</param>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        /// <param name="notyf">Notification service.</param>
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

        /// <summary>
        /// Displays the registration view.
        /// </summary>
        /// <returns>The registration view.</returns>
        public IActionResult Register()
        {
            var response = new RegisterationViewModel();
            return View(response);
        }

        /// <summary>
        /// Displays the login view.
        /// </summary>
        /// <returns>The login view.</returns>
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        /// <summary>
        /// Logs out the currently signed-in user.
        /// </summary>
        /// <returns>Redirects to the home page after logout.</returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Handles user registration.
        /// </summary>
        /// <param name="registerVM">Registration view model containing user details.</param>
        /// <returns>Redirects to the home page if registration is successful; otherwise, returns to the registration view.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterationViewModel registerVM)
        {
            // Check if the provided model is valid
            if (ModelState.IsValid)
            {
                // Check if the user with the provided email already exists
                var userExists = await _userManager.FindByEmailAsync(registerVM.Email);

                if (userExists == null)
                {
                    // Create a new ApplicationUser object
                    var appUser = new ApplicationUser { UserName = registerVM.Email, Email = registerVM.Email };

                    // Attempt to create the user
                    var result = await _userManager.CreateAsync(appUser, registerVM.Password);

                    if (result.Succeeded)
                    {
                        // Sign in the user and redirect to the home page
                        await _signInManager.SignInAsync(appUser, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    // If user creation fails, add errors to the model state
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    // If the user already exists, show an error message
                    TempData["Error"] = "User already exists";
                    return View(registerVM);
                }
            }

            // If the model state is invalid, return to the registration view with an error message
            TempData["Error"] = "Registration failed";
            return View(registerVM);
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        /// <param name="loginVM">Login view model containing user credentials.</param>
        /// <returns>Redirects to the appropriate dashboard if login is successful; otherwise, returns to the login view.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid) return View(loginVM);

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            // Determine the user role
            var role = await DetermineUserRole(user);

            // Check if the user exists and the password is correct
            if (user != null && await _userManager.CheckPasswordAsync(user, loginVM.Password))
            {
                // Sign in the user
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    // Display a success notification and redirect to the appropriate dashboard
                    _notyf.Success($"{user.FirstName} is logged in");
                    return RedirectToAction($"{role}Dashboard", "Dashboard");
                }
            }

            // If login fails, display an error notification and return to the login view
            _notyf.Error("Wrong credentials. Please enter the correct credentials");
            return View(loginVM);
        }

        /// <summary>
        /// Determines the role of the provided user asynchronously.
        /// </summary>
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
