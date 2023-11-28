using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.Repository
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
            var drivers = _userManager.GetUsersInRoleAsync(Roles.Driver.ToString()).Result.ToList();
            return drivers;
        }

        public Task<IEnumerable<ApplicationUser>> GetPassengers()
        {
            throw new NotImplementedException();
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
