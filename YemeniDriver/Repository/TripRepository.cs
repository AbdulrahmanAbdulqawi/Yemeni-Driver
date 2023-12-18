using Microsoft.EntityFrameworkCore;
using YemeniDriver.Interfaces;
using YemeniDriver.Data;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TripRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(Trip trip)
        {
            _dbContext.Add(trip);
            return Save();
        }

        public bool Delete(Trip trip)
        {
            _dbContext.Remove(trip);
            return Save();
        }

        public async Task<IEnumerable<Trip>> GetAll()
        {
            return await _dbContext.Trips.ToListAsync();       
        }

        public async Task<Trip> GetByIdAsync(string id)
        {
            return await _dbContext.Trips.FirstOrDefaultAsync(a => a.TripId == id);
        }

        public async Task<Trip> GetByIdAsyncNoTracking(string id)
        {
            return await _dbContext.Trips.AsNoTracking().FirstOrDefaultAsync(a => a.TripId == id);
        }

        public async Task<IEnumerable<Trip>> GetByUserId(string userId, Roles role)
        {
            if (role == Roles.Driver)
            {
                return await _dbContext.Trips.Where(a => a.DriverId == userId).ToListAsync();
            }
            return await _dbContext.Trips.Where(a => a.PassengerId == userId).ToListAsync();
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Trip trip)
        {
            _dbContext.Trips.Update(trip);
            return Save();
        }

        public bool DeleteRange(List<Trip> trips)
        {
            _dbContext.Trips.RemoveRange(trips);
            return Save();
        }
    }
}
