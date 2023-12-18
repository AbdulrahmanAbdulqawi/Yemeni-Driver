using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public DashboardRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _userRepository = userRepository;
        }
        public async Task<ApplicationUser> GetDriverByIdAsync(string driverId)
        {
            var driver = await _userRepository.GetByIdAsyncNoTracking(driverId);
            if (driver != null)
            {
                return driver;
            }
            throw new Exception("Driver not found");

        }

        public Task<ApplicationUser> GetDriverByIdAsyncNoTracking(string driverId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetDrivers()
        {
           return await _userRepository.GetDrivers();
        }

        public async Task<ApplicationUser> GetPassengerByIdAsync(string passengerId)
        {
            var passenger = _userManager.GetUsersInRoleAsync(Roles.Passenger.ToString()).Result.FirstOrDefault(a => a.Id == passengerId);
            if (passenger != null) return passenger;
            throw new Exception("passenger not found");
        }

        public Task<ApplicationUser> GetPassengerByIdAsyncNoTracking(string passengerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetPassengers()
        {
            return await _userManager.GetUsersInRoleAsync(Roles.Passenger.ToString());
        }
    }
}
