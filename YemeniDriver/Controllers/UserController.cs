using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreHero.ToastNotification.Abstractions;
using YemeniDriver.Models;
using YemeniDriver.Interfaces;
using YemeniDriver.ViewModel.Vehicle;
using YemeniDriver.Data;
using YemeniDriver.Data.Enums;
using Microsoft.EntityFrameworkCore;
using GoogleMapsApi.Entities.Directions.Response;
using YemeniDriver.ViewModel.User;

namespace YemeniDriver.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly HttpContextAccessor _contextAccessor;

        // Services for photo upload and notification
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDriverAndRequestRepository _driverAndRequestRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ITripRepository _tripRepository;
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
        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IPhotoService photoService,
            IVehicleRepository vehicleRepository,
            INotyfService notyf,
            IDriverAndRequestRepository driverAndRequestRepository,
            IRequestRepository requestRepository,
            ITripRepository tripRepository,
            HttpContextAccessor contextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _notyf = notyf;
            _driverAndRequestRepository = driverAndRequestRepository;
            _requestRepository = requestRepository;
            _tripRepository = tripRepository;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the driver registration view.
        /// </summary>
        /// <returns>The driver registration view.</returns>
        public IActionResult RegisterAsDriver()
        {
            return View(new DriverRegisterationViewModel());
        }

        /// <summary>
        /// Handles driver registration.
        /// </summary>
        /// <param name="driverRegisterVM">Driver registration view model containing driver details.</param>
        /// <returns>Redirects to the driver dashboard if registration is successful; otherwise, returns to the driver registration view.</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterAsDriver(DriverRegisterationViewModel driverRegisterVM)
        {
            // Check if the user with the provided email already exists
            var userExists = await CheckIfUserExistsAsync(driverRegisterVM.Email);

            if (userExists != null)
            {
                // If the user already exists, show an error message
                TempData["Error"] = "User already exists!";
                return View(driverRegisterVM);
            }

            // Create a new ApplicationUser object for the driver
            var appUser = await CreateApplicationUserAsync(driverRegisterVM);
            appUser.Roles = Roles.Driver;

// Create a new Vehicle object for the driver
            var newVehicle = await CreateVehicleAsync(driverRegisterVM, appUser.Id);

            // Attempt to create the user and assign the driver role
            var result = await CreateUserAndAssignRoleAsync(appUser, newVehicle, Roles.Driver, driverRegisterVM.Password);


            if (result.Succeeded)
            {
                // Display a success notification and redirect to the driver dashboard
                _notyf.Success("Driver account created successfully");
                return RedirectToAction("DriverDashboard", "Dashboard");
            }

            // If user creation fails, add errors to the model state
            AddErrorsToModelState(result.Errors);
            _notyf.Error("Registration Failed");
            return View(driverRegisterVM);
        }

        /// <summary>
        /// Displays the passenger registration view.
        /// </summary>
        /// <returns>The passenger registration view.</returns>
        public IActionResult RegisterAsPassenger()
        {
            return View(new PassengerRegisterationViewModel());
        }

        /// <summary>
        /// Handles passenger registration.
        /// </summary>
        /// <param name="registerVM">Passenger registration view model containing passenger details.</param>
        /// <returns>Redirects to the passenger dashboard if registration is successful; otherwise, returns to the passenger registration view.</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterAsPassenger(PassengerRegisterationViewModel registerVM)
        {
            // Check if the user with the provided email already exists
            var userExists = await CheckIfUserExistsAsync(registerVM.Email);

            if (userExists != null)
            {
                // If the user already exists, show an error message
                TempData["Error"] = "User already exists!";
                return View(registerVM);
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, show an error message
                TempData["Error"] = "Entries are invalid!";
                return View(registerVM);
            }

            // Create a new ApplicationUser object for the passenger
            var appUser = await CreateApplicationUserAsync(registerVM);

            // Attempt to create the user and assign the passenger role
            var result = await CreateUserAndAssignRoleAsync(appUser, null, Roles.Passenger, registerVM.Password);

            if (result.Succeeded)
            {
                // Display a success notification and redirect to the passenger dashboard
                _notyf.Success("Passenger account created successfully");
                return RedirectToAction("PassengerDashboard", "Dashboard");
            }

            // If user creation fails, add errors to the model state
            AddErrorsToModelState(result.Errors);
            _notyf.Error("Registration Failed");
            return View(registerVM);
        }
        /// <summary>
        /// Displays driver details including associated vehicle details.
        /// </summary>
        /// <param name="driverId">ID of the driver.</param>
        /// <returns>The driver details view.</returns>
        public async Task<IActionResult> ViewDriverDetails(string driverId)
        {
            // Retrieve user and vehicle details for the specified driver ID
            var driver = await GetUserDetailsAsync(driverId);
            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            driver.Vehicle = vehicle;


            if (driver == null)
            {
                // If the driver is not found, return a not found result
                return NotFound();
            }

            // Map the user and vehicle details to the driver details view model
            var driverVM = MapToDriverDetailsViewModel(driver);

            // Display the driver details view
            return View(driverVM);
        }

        /// <summary>
        /// Displays passenger details.
        /// </summary>
        /// <param name="passengerId">ID of the passenger.</param>
        /// <returns>The passenger details view.</returns>
        public async Task<IActionResult> ViewPassengerDetails(string passengerId)
        {
            // Retrieve user details for the specified passenger ID
            var passenger = await GetUserDetailsAsync(passengerId);

            if (passenger == null)
            {
                // If the passenger is not found, return a not found result
                return NotFound();
            }

            // Map the user details to the passenger details view model
            var passengerVM = MapToPassengerDetailsViewModel(passenger);

            // Display the passenger details view
            return View(passengerVM);
        }

        /// <summary>
        /// Displays the view for editing driver details.
        /// </summary>
        /// <param name="driverId">ID of the driver.</param>
        /// <returns>The edit driver details view.</returns>
        public async Task<IActionResult> EditDriverDetails(string driverId)
        {
            // Retrieve user and vehicle details for the specified driver ID
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);

            driver.Vehicle = vehicle;

            if (driver == null)
            {
                // If the driver is not found, return a not found result
                return NotFound(driverId);
            }

            // Map the user and vehicle details to the edit driver details view model
            var editDriverDetailsVM = MapToEditDriverDetailsViewModel(driver);

            // Display the edit driver details view
            return View(editDriverDetailsVM);
        }

        /// <summary>
        /// Handles the submission of edited driver details.
        /// </summary>
        /// <param name="driverId">ID of the driver.</param>
        /// <param name="editDriverDetailsViewModel">View model containing edited driver details.</param>
        /// <returns>Redirects to the driver dashboard if the update is successful; otherwise, returns an error view.</returns>
        [HttpPost]
        /// <summary>
        /// Handles the submission of edited driver details.
        /// </summary>
        /// <param name="driverId">ID of the driver.</param>
        /// <param name="editDriverDetailsViewModel">View model containing edited driver details.</param>
        /// <returns>Redirects to the driver dashboard if the update is successful; otherwise, returns an error view.</returns>
        [HttpPost]
        public async Task<IActionResult> EditDriverDetails(string driverId, EditDriverDetailsViewModel editDriverDetailsViewModel)
        {
            // Retrieve user and vehicle details for the specified driver ID
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);

            if (driver == null)
            {
                // If the driver is not found, return a not found result
                return NotFound(driverId);
            }

            // Remove the "Vehicle.ApplicationUser" ModelState entry
            ModelState.Remove("Vehicle.ApplicationUser");

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, add an error to the model state and return an error view
                ModelState.AddModelError("", "Failed to update driver");
                return View("Error", editDriverDetailsViewModel);
            }

            try
            {
                // Delete existing profile and vehicle photos
                await DeletePhotosAsync(driver.ProfileImageUrl, driver.Vehicle?.VehiclImageUrl);

                // Upload new profile and vehicle photos
                var profileImageResult = await _photoService.AddPhotoAsync(editDriverDetailsViewModel.ProfileImage);
                var vehicleImageResult = await _photoService.AddPhotoAsync(editDriverDetailsViewModel.VehicleImage);

                // Update driver and vehicle details
                UpdateDriverAndVehicle(driver, editDriverDetailsViewModel, profileImageResult.Url.ToString(), vehicleImageResult.Url.ToString());

                // Display a success notification and redirect to the driver dashboard
                _notyf.Success("Update Success");
                return RedirectToAction("DriverDashboard", "Dashboard");
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                throw;
            }
        }


        /// <summary>
        /// Displays the view for editing passenger details.
        /// </summary>
        /// <param name="passengerId">ID of the passenger.</param>
        /// <returns>The edit passenger details view.</returns>
        public async Task<IActionResult> EditPassengerDetails(string passengerId)
        {
            // Retrieve user details for the specified passenger ID
            var passenger = await GetUserDetailsAsync(passengerId);

            if (passenger == null)
            {
                // If the passenger is not found, return a not found result
                return NotFound(passengerId);
            }

            // Map the user details to the edit passenger details view model
            var editPassengerDetailsVM = MapToEditPassengerDetailsViewModel(passenger);

            // Display the edit passenger details view
            return View(editPassengerDetailsVM);
        }

        /// <summary>
        /// Handles the submission of edited passenger details.
        /// </summary>
        /// <param name="passengerId">ID of the passenger.</param>
        /// <param name="editPassengerDetailsViewModel">View model containing edited passenger details.</param>
        /// <returns>Redirects to the passenger dashboard if the update is successful; otherwise, returns an error view.</returns>
        [HttpPost]
        public async Task<IActionResult> EditPassengerDetails(string passengerId, EditPassengerDetailsViewModel editPassengerDetailsViewModel)
        {
            // Retrieve user details for the specified passenger ID
            var passenger = await GetUserDetailsAsync(passengerId);

            if (passenger == null)
            {
                // If the passenger is not found, return a not found result
                return NotFound(passengerId);
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, add an error to the model state and return an error view
                ModelState.AddModelError("", "Failed to update passenger");
                return View("Error", editPassengerDetailsViewModel);
            }

            try
            {
                // Delete existing profile photo
                await DeletePhotoAsync(passenger.ProfileImageUrl);

                // Upload new profile photo
                var profileImageResult = await _photoService.AddPhotoAsync(editPassengerDetailsViewModel.ProfileImage);

                // Update passenger details
                UpdatePassenger(passenger, editPassengerDetailsViewModel, profileImageResult.Url.ToString());

                // Display a success notification and redirect to the passenger dashboard
                _notyf.Success("Update Success");
                return RedirectToAction("PassengerDashboard", "Dashboard");
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userRepository.GetByIdAsyncNoTracking(userId);
            var currentUserId = _contextAccessor.HttpContext.User.GetUserId();
            var currentUser = await _userRepository.GetByIdAsyncNoTracking(currentUserId);


            try
            {
                if (currentUser.Roles != Roles.Admin)
                {
                    if (user.Roles == Roles.Driver)
                    {
                        var vehicle = await _vehicleRepository.GetVehicleByOwner(userId);
                        var driverAndRequest = _driverAndRequestRepository.GetDriverAndRequestAsync();
                        var request = _requestRepository.GetByDriverId(userId);

                        if (vehicle != null)
                        {
                            await DeletePhotoAsync(vehicle.VehiclImageUrl);
                            _vehicleRepository.Delete(vehicle);
                        }
                        await DeletePhotoAsync(user.ProfileImageUrl);

                        _userRepository.Delete(user);
                        await _signInManager.SignOutAsync();

                    }
                    else if (user.Roles == Roles.Passenger)
                    {
                        _userRepository.Delete(user);
                        await _signInManager.SignOutAsync();
                    }
                    return RedirectToAction("Index", "Home");

                }

                else
                {
                    _userRepository.Delete(user);
                    return RedirectToAction("AdminDashboard", "Dashboard");
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        /// <summary>
        /// Retrieves user details based on the provided user ID asynchronously.
        /// </summary>
        private async Task<ApplicationUser> GetUserDetailsAsync(string userId)
        {
            return await _userRepository.GetByIdAsyncNoTracking(userId);
        }

   

        /// <summary>
        /// Deletes a photo from the storage service based on the provided image URL asynchronously.
        /// </summary>
        private async Task DeletePhotoAsync(string imageUrl)
        {
            if (imageUrl != null)
            {
                await _photoService.DeletePhotoAsync(imageUrl);
            }
        }

        /// <summary>
        /// Deletes user profile and vehicle photos from the storage service based on the provided image URLs asynchronously.
        /// </summary>
        private async Task DeletePhotosAsync(string profileImageUrl, string vehicleImageUrl)
        {
            await DeletePhotoAsync(profileImageUrl);
            if (vehicleImageUrl != null)
            {
                await DeletePhotoAsync(vehicleImageUrl);
            }
        }

        /// <summary>
        /// Updates driver details and associated vehicle details based on the provided data.
        /// </summary>
        private void UpdateDriverAndVehicle(ApplicationUser driver, EditDriverDetailsViewModel editDriverDetailsViewModel, string profileImageUrl, string vehicleImageUrl)
        {
            driver.FirstName = editDriverDetailsViewModel.FirstName;
            driver.LastName = editDriverDetailsViewModel.LastName;
            driver.Email = editDriverDetailsViewModel.Email;
            driver.Gender = editDriverDetailsViewModel.Gender;
            driver.PhoneNumber = editDriverDetailsViewModel.PhoneNumber;
            driver.ProfileImageUrl = profileImageUrl;
            _userRepository.Update(driver);


            var vehicletoUpdate = editDriverDetailsViewModel.Vehicle;
            vehicletoUpdate.ApplicationUserId = driver.Id;
            vehicletoUpdate.VehicleId = vehicletoUpdate.VehicleId;
            vehicletoUpdate.VehiclImageUrl = vehicleImageUrl;
            _vehicleRepository.Update(vehicletoUpdate);
        }

        /// <summary>
        /// Updates passenger details based on the provided data.
        /// </summary>
        private void UpdatePassenger(ApplicationUser passenger, EditPassengerDetailsViewModel model, string profileImageUrl)
        {
            passenger.FirstName = model.FirstName;
            passenger.LastName = model.LastName;
            passenger.Email = model.Email;
            passenger.Gender = model.Gender;
            passenger.PhoneNumber = model.PhoneNumber;
            passenger.ProfileImageUrl = profileImageUrl;

            _userRepository.Update(passenger);
        }

        /// <summary>
        /// Maps driver details to a view model suitable for displaying.
        /// </summary>
        private DriverDetailsViewModel MapToDriverDetailsViewModel(ApplicationUser driver)
        {
            return new DriverDetailsViewModel()
            {
                Email = driver.Email,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Image = driver.ProfileImageUrl,
                Location = driver.Location,
                PhoneNumber = driver.PhoneNumber,
                Rating = 5,
                ViewVehicle = new ViewVehicleViewModel
                {
                    Capacity = driver.Vehicle?.Capacity,
                    Color = driver.Vehicle?.Color,
                    Make = driver.Vehicle?.Make,
                    Model = driver.Vehicle?.Model,
                    PlateNumber = driver.Vehicle?.PlateNumber,
                    VehiclImageUrl = driver.Vehicle?.VehiclImageUrl,
                    Year = driver.Vehicle?.Year,
                }
            };
        }

        /// <summary>
        /// Maps passenger details to a view model suitable for displaying.
        /// </summary>
        private PassengerDetailsViewModel MapToPassengerDetailsViewModel(ApplicationUser passenger)
        {
            return new PassengerDetailsViewModel()
            {
                Email = passenger.Email,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Image = passenger.ProfileImageUrl,
                Location = passenger.Location,
                PhoneNumber = passenger.PhoneNumber,
                Rating = 5,
            };
        }

        /// <summary>
        /// Maps driver details to a view model suitable for editing.
        /// </summary>
        private EditDriverDetailsViewModel MapToEditDriverDetailsViewModel(ApplicationUser driver)
        {
            return new EditDriverDetailsViewModel()
            {
                DrivingLicenseNumber = driver.DrivingLicenseNumber,
                Email = driver.Email,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Gender = (Gender)driver.Gender,
                ProfileImageUrl = driver.ProfileImageUrl,
                PhoneNumber = driver.PhoneNumber,

                Vehicle = new YemeniDriver.Models.Vehicle
                {
                    VehicleId = driver.Vehicle?.VehicleId,
                    ApplicationUserId = driver.Vehicle?.ApplicationUserId,
                    Capacity = driver.Vehicle?.Capacity,
                    Color = driver.Vehicle?.Color,
                    Make = driver.Vehicle?.Make,
                    Model = driver.Vehicle?.Model,
                    PlateNumber = driver.Vehicle?.PlateNumber,
                    VehiclImageUrl = driver.Vehicle?.VehiclImageUrl,
                    Year = driver.Vehicle?.Year,
                }
            };
        }

        /// <summary>
        /// Maps passenger details to a view model suitable for editing.
        /// </summary>
        private EditPassengerDetailsViewModel MapToEditPassengerDetailsViewModel(ApplicationUser passenger)
        {
            return new EditPassengerDetailsViewModel()
            {
                DrivingLicenseNumber = passenger.DrivingLicenseNumber,
                Email = passenger.Email,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Gender = (Gender)passenger.Gender,
                ProfileImageUrl = passenger.ProfileImageUrl,
                PhoneNumber = passenger.PhoneNumber,
            };
        }

       

        /// <summary>
        /// Checks if a user with the provided email already exists asynchronously.
        /// </summary>
        private async Task<ApplicationUser> CheckIfUserExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Creates an application user based on the provided registration view model asynchronously.
        /// </summary>
        private async Task<ApplicationUser> CreateApplicationUserAsync(RegisterationBaseViewModel registerVM)
        {
            var profileImageUploadResult = await _photoService.AddPhotoAsync(registerVM.ProfileImage);

            return new ApplicationUser
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Gender = registerVM.Gender,
                PhoneNumber = registerVM.PhoneNumber,
                ProfileImageUrl = profileImageUploadResult.Url.ToString(),
                Roles = Roles.Passenger, // Default role for now
                LiveLocationLongitude = 10,
                LiveLocationLatitude = 10,
            };
        }

        /// <summary>
        /// Creates a vehicle based on the provided driver registration view model and user ID asynchronously.
        /// </summary>
        private async Task<Models.Vehicle> CreateVehicleAsync(DriverRegisterationViewModel driverRegisterVM, string userId)
        {
            var vehicleImageUploadResult = await _photoService.AddPhotoAsync(driverRegisterVM.VehicleImage);

            return new YemeniDriver.Models.Vehicle
            {
                VehicleId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                ApplicationUserId = userId,
                Capacity = driverRegisterVM.Capacity,
                Color = driverRegisterVM.Color,
                Model = driverRegisterVM.Model,
                Year = driverRegisterVM.Year,
                Make = driverRegisterVM.Make,
                PlateNumber = driverRegisterVM.PlateNumber,
                VehiclImageUrl = vehicleImageUploadResult.Url.ToString()
            };
        }

        /// <summary>
        /// Creates a user, assigns a role, and signs in based on the provided data asynchronously.
        /// </summary>
        private async Task<IdentityResult> CreateUserAndAssignRoleAsync(ApplicationUser appUser, YemeniDriver.Models.Vehicle vehicle, Roles role, string password)
        {
            var result = await _userManager.CreateAsync(appUser, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(appUser, role.ToString());

                if (vehicle != null)
                {
                    appUser.Vehicle = vehicle;
                    _vehicleRepository.Add(vehicle);
                }

                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }

            return result;
        }
        /// <summary>
        /// Adds errors to the ModelState based on the provided identity errors.
        /// </summary>
        private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

    }
}
