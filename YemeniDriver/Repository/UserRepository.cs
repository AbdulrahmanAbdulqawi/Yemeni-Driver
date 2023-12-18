using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        public bool Add(ApplicationUser user)
        {
            _applicationDbContext.Add(user);
            return Save();
        }

        public bool Delete(ApplicationUser user)
        {
            _applicationDbContext.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await _applicationDbContext.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _applicationDbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ApplicationUser> GetByIdAsyncNoTracking(string id)
        {
            return await _applicationDbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetDrivers()
        {
            var drivers = await _userManager.GetUsersInRoleAsync(Roles.Driver.ToString());

            if(!drivers.Any(a => a.Roles == Roles.Driver))
            {
                throw new Exception("No Driver Found");
            }
            return drivers.ToList();
        }

        public async Task<IEnumerable<ApplicationUser>> GetPassengers()
        {
            var passengers = await _userManager.GetUsersInRoleAsync(Roles.Passenger.ToString());

            if (!passengers.Any(a => a.Roles == Roles.Driver))
            {
                throw new Exception("No Passenger Found");
            }
            return passengers.ToList();
        }

        public async Task<List<(double?, double?)>> GetUserLocation(string userId)
        {
            var userLocation = await _applicationDbContext.Users.Where(u => u.Id == userId && u.LiveLocationLatitude != null && u.LiveLocationLongitude != null)
                .Select(a => new
            {
                a.LiveLocationLongitude,
                a.LiveLocationLatitude,
            }).ToListAsync();

            var resultList = userLocation.Select(ll => (ll.LiveLocationLatitude, ll.LiveLocationLongitude)).ToList();

            return resultList;
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ApplicationUser user)
        {
            _applicationDbContext.Update(user);
            return Save();
        }
    }
}
