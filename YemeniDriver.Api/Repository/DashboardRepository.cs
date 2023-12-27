using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using YemeniDriver.Api.Data;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Repository
{
    /// <summary>
    /// Repository for handling dashboard-related data operations.
    /// </summary>
    public class DashboardRepository : IDashboardRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DashboardRepository> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Constructor for the DashboardRepository.
        /// </summary>
        public DashboardRepository(UserManager<ApplicationUser> userManager, IUserRepository userRepository, ILogger<DashboardRepository> logger, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetDriverByIdAsync(string driverId)
        {
            try
            {
                var driver = await _userRepository.GetDriverById(driverId);
                if (driver != null)
                {
                    return driver;
                }
                throw new Exception("Driver not found");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error retrieving driver by ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetDriverByIdAsyncNoTracking(string driverId)
        {
            // Implement this method if needed
            try
            {
                var driver = await _userRepository.GetDriverById(driverId);
                if (driver != null)
                {
                    return driver;
                }
                throw new Exception("Driver not found");
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error retrieving driver by ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ApplicationUser>> GetDrivers()
        {
            try
            {
                return await _userRepository.GetDrivers();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving list of drivers");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetPassengerByIdAsync(string passengerId)
        {
            try
            {
                var passenger = await _userRepository.GetPassengerById(passengerId);
                if (passenger != null)
                {
                    return passenger;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving passenger by ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetPassengerByIdAsyncNoTracking(string passengerId)
        {
            // Implement this method if needed
            try
            {
                var passenger = await _userRepository.GetPassengerById(passengerId);
                if (passenger != null)
                {
                    return passenger;
                }
                throw new Exception("Passenger not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving passenger by ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ApplicationUser>> GetPassengers()
        {
            try
            {
                return await _userRepository.GetPassengers();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error retrieving list of passengers");
                throw;
            }
        }
    }
}
