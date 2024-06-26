﻿using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<IEnumerable<ApplicationUser>> GetDrivers();
        Task<IEnumerable<ApplicationUser>> GetPassengers();
        Task<List<(double?, double?)>> GetUserLocation(string userId); 
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByIdAsyncNoTracking(string id);
        bool Add(ApplicationUser user);
        bool Update(ApplicationUser user);
        bool Delete(ApplicationUser user);
        bool Save();
    }
}
