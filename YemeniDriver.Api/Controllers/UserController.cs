using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YemeniDriver.Api.ViewModel.User;
using YemeniDriver.Api.Models;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.ViewModel.Vehicle;
using Microsoft.AspNetCore.Authorization;

namespace YemeniDriver.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IPhotoService photoService,
            IVehicleRepository vehicleRepository,
            IRequestRepository requestRepository,
            ITripRepository tripRepository,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _requestRepository = requestRepository;
            _tripRepository = tripRepository;
            _contextAccessor = contextAccessor;
        } 

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello from UserController!");
        }

        [HttpPost("registerAsDriver")]
        public async Task<IActionResult> RegisterAsDriver(DriverRegisterationViewModel driverRegisterVM)
        {
            var userExists = await CheckIfUserExistsAsync(driverRegisterVM.Email);

            if (userExists != null)
            {
                return BadRequest("User already exists!");
            }

            var appUser = await CreateApplicationUserAsync(driverRegisterVM);
            appUser.Roles = Roles.Driver;

            var newVehicle = await CreateVehicleAsync(driverRegisterVM, appUser.Id);

            var result = await CreateUserAndAssignRoleAsync(appUser, newVehicle, Roles.Driver, driverRegisterVM.Password);

            if (result.Succeeded)
            {
                return Ok("Driver account created successfully");
            }

            return BadRequest("Registration Failed");
        }

        [HttpPost("registerAsPassenger")]
        public async Task<IActionResult> RegisterAsPassenger(PassengerRegisterationViewModel registerVM)
        {
            var userExists = await CheckIfUserExistsAsync(registerVM.Email);

            if (userExists != null)
            {
                return BadRequest("User already exists!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Entries are invalid!");
            }

            var appUser = await CreateApplicationUserAsync(registerVM);

            var result = await CreateUserAndAssignRoleAsync(appUser, null, Roles.Passenger, registerVM.Password);

            if (result.Succeeded)
            {
                return Ok("Passenger account created successfully");
            }

            return BadRequest("Registration Failed");
        }
        

        [HttpGet("viewDriverDetails/{driverId}")]
        [Authorize(Roles = "Driver")] // Add this attribute to make the method accessible only by users with the "Passenger" role
        public async Task<IActionResult> ViewDriverDetails(string driverId)
        {
            var driver = await _userRepository.GetDriverById(driverId);

            if (driver == null)
            {
                return NotFound($"{driverId} does not exist");
            }

            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            if (vehicle != null)
            {
                driver.Vehicle = vehicle;
            }



            var driverVM = MapToDriverDetailsViewModel(driver);

            return Ok(driverVM);
        }

        [HttpGet("viewPassengerDetails/{passengerId}")]
        [Authorize(Roles = "Passenger")] // Add this attribute to make the method accessible only by users with the "Passenger" role
        public async Task<IActionResult> ViewPassengerDetails(string passengerId)
        {
            var passenger = await _userRepository.GetPassengerById(passengerId);

            if (passenger == null)
            {
                return NotFound($"{passengerId} does not exist");
            }

            var passengerVM = MapToPassengerDetailsViewModel(passenger);

            return Ok(passengerVM);
        }

        [HttpGet("editDriverDetails/{driverId}")]
        [Authorize(Roles = "Driver")] // Add this attribute to make the method accessible only by users with the "Passenger" role
        public async Task<IActionResult> EditDriverDetails(string driverId)
        {
            var driver = await _userRepository.GetDriverById(driverId);

            if (driver == null)
            {
                return NotFound($"{driverId} does not exist");
            }

            var vehicle = await _vehicleRepository.GetVehicleByOwner(driverId);
            if(vehicle != null)
            {
                driver.Vehicle = vehicle;
            }

            var editDriverDetailsVM = MapToEditDriverDetailsViewModel(driver);

            return Ok(editDriverDetailsVM);
        }

        [HttpPost("editDriverDetails/{driverId}")]
        [Authorize(Roles = "Driver")] // Add this attribute to make the method accessible only by users with the "Passenger" role
        public async Task<IActionResult> EditDriverDetails(string driverId, EditDriverDetailsViewModel editDriverDetailsViewModel)
        {
            var driver = await _userRepository.GetDriverById(driverId);

            if (driver == null)
            {
                return NotFound($"{driverId} does not exist");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to update driver");
                return BadRequest("Failed to update driver");
            }

            try
            {
                await DeletePhotosAsync(driver.ProfileImageUrl, driver.Vehicle?.VehiclImageUrl);

                var profileImageResult = await _photoService.AddPhotoAsync(editDriverDetailsViewModel.ProfileImage);
                var vehicleImageResult = await _photoService.AddPhotoAsync(editDriverDetailsViewModel.VehicleImage);

                UpdateDriverAndVehicle(driver, editDriverDetailsViewModel, profileImageResult.Url.ToString(), vehicleImageResult.Url.ToString());

                return Ok("Update Success");
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet("editPassengerDetails/{passengerId}")]
        [Authorize(Roles = "Passenger")] 
        public async Task<IActionResult> EditPassengerDetails(string passengerId)
        {
            var passenger = await _userRepository.GetPassengerById(passengerId);

            if (passenger == null)
            {
                return NotFound($"{passengerId} does not exist");
            }

            var editPassengerDetailsVM = MapToEditPassengerDetailsViewModel(passenger);

            return Ok(editPassengerDetailsVM);
        }

        [HttpPost("editPassengerDetails/{passengerId}")]
        [Authorize(Roles = "Passenger")] 
        public async Task<IActionResult> EditPassengerDetails(string passengerId, EditPassengerDetailsViewModel editPassengerDetailsViewModel)
        {
            var passenger = await _userRepository.GetPassengerById(passengerId);

            if (passenger == null)
            {
                return NotFound($"{passengerId} does not exist");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to update passenger");
                return BadRequest("Failed to update passenger");
            }

            try
            {
                await DeletePhotoAsync(passenger.ProfileImageUrl);

                var profileImageResult = await _photoService.AddPhotoAsync(editPassengerDetailsViewModel.ProfileImage);

                UpdatePassenger(passenger, editPassengerDetailsViewModel, profileImageResult.Url.ToString());

                return Ok("Update Success");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("deleteUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await GetUserDetailsAsync(userId);
            ApplicationUser? currentUser = null;

            var isAuthenticated = _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {
                var currentUserId = _contextAccessor.HttpContext.User.GetUserId();
                currentUser = await GetUserDetailsAsync(currentUserId);

            }
            var vehicle = await _vehicleRepository.GetVehicleByOwner(userId);

            try
            {
                if (currentUser != null && currentUser.Roles != Roles.Admin)
                {
                    if (user.Roles == Roles.Driver)
                    {
                        var trips = await _tripRepository.GetByUserId(userId, Roles.Driver);

                        if (trips != null) _tripRepository.DeleteTrips(trips.ToList());

                        var requests = await _requestRepository.GetByUserId(userId, Roles.Driver);

                        if (requests != null) _requestRepository.DeleteRequests(requests.ToList());

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
                        var trips = await _tripRepository.GetByUserId(userId, Roles.Passenger);

                        if (trips != null) _tripRepository.DeleteTrips(trips.ToList());

                        var requests = await _requestRepository.GetByUserId(userId, Roles.Passenger);

                        if (requests != null) _requestRepository.DeleteRequests(requests.ToList());

                        _userRepository.Delete(user);
                        await _signInManager.SignOutAsync();
                    }

                    return Ok("User deleted successfully");
                }
                else
                {
                    if (user.Roles == Roles.Driver)
                    {
                        var trips = await _tripRepository.GetByUserId(userId, Roles.Driver);

                        if (trips != null) _tripRepository.DeleteTrips(trips.ToList());

                        var requests = await _requestRepository.GetByUserId(userId, Roles.Driver);

                        if (requests != null) _requestRepository.DeleteRequests(requests.ToList());

                        if (vehicle != null)
                        {
                            await DeletePhotoAsync(vehicle.VehiclImageUrl);
                            _vehicleRepository.Delete(vehicle);
                        }

                        await DeletePhotoAsync(user.ProfileImageUrl);

                        _userRepository.Delete(user);
                    }
                    else if (user.Roles == Roles.Passenger)
                    {
                        var trips = await _tripRepository.GetByUserId(userId, Roles.Passenger);

                        if (trips != null) _tripRepository.DeleteTrips(trips.ToList());

                        var requests = await _requestRepository.GetByUserId(userId, Roles.Passenger);

                        if (requests != null) _requestRepository.DeleteRequests(requests.ToList());

                        _userRepository.Delete(user);
                    }

                    return Ok("User deleted successfully");
                }
            }
            catch (Exception)
            {
                // Handle exceptions appropriately
                return BadRequest("Failed to delete user");
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
            if(profileImageUrl != null)
            {
                await DeletePhotoAsync(profileImageUrl);
            }
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

            Vehicle vehicle = new Vehicle
            {
                VehicleId = vehicletoUpdate.VehicleId,
                DriverId = vehicletoUpdate.DriverId,
                VehiclImageUrl = vehicleImageUrl
            };

            _vehicleRepository.Update(vehicle);
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
                Rating = (int?)driver.Rating ?? 5,
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
            var passengerVM = new PassengerDetailsViewModel()
            {
                Email = passenger.Email,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Image = passenger.ProfileImageUrl,
                Location = passenger.Location,
                PhoneNumber = passenger.PhoneNumber,
                Rating = (int?)passenger.Rating ?? 5,

            };

            return passengerVM;
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

                Vehicle = new ViewVehicleViewModel
                {
                    VehicleId = driver.Vehicle?.VehicleId,
                    DriverId = driver.Vehicle?.DriverId,
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

            return new Models.Vehicle
            {
                VehicleId = $"{DateTime.Now:yyyyMMddHHmmssfff}-{RandomString(4)}",
                DriverId = userId,
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
        private async Task<IdentityResult> CreateUserAndAssignRoleAsync(ApplicationUser appUser, Vehicle vehicle, Roles role, string password)
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
