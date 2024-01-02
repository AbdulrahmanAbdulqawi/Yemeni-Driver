using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    /// <summary>
    /// Repository for handling dashboard-related data operations.
    /// </summary>
    public class DashboardRepository : IDashboardRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DashboardRepository> _logger;

        /// <summary>
        /// Constructor for the DashboardRepository.
        /// </summary>
        public DashboardRepository( UserManager<ApplicationUser> userManager, IUserRepository userRepository, ILogger<DashboardRepository> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetDriverByIdAsync(string driverId)
        {
            try
            {
                var driver = await _userRepository.GetByIdAsync(driverId);
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
                var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
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
                var passenger = await _userRepository.GetByIdAsync(passengerId);
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
        public async Task<ApplicationUser> GetPassengerByIdAsyncNoTracking(string passengerId)
        {
            // Implement this method if needed
            try
            {
                var passenger = await _userRepository.GetByIdAsyncNoTracking(passengerId);
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
