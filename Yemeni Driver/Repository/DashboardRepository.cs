using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetDriverByIdAsync(string driverId)
        {
            var driver = _userManager.GetUsersInRoleAsync(Roles.Driver.ToString()).Result.FirstOrDefault(a => a.Id == driverId);
            if(driver != null)return driver;
            throw new Exception("Driver not found");
        }

        public Task<ApplicationUser> GetDriverByIdAsyncNoTracking(string driverId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUser>> GetDrivers()
        {
           return await _userManager.GetUsersInRoleAsync(Roles.Driver.ToString());
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

        public Task<IEnumerable<ApplicationUser>> GetPassengers()
        {
            throw new NotImplementedException();
        }
    }
}
