using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Interfaces;
using YemeniDriver.Data;
using YemeniDriver.Models;

namespace Yemeni_Driver.Repository
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
    }
}
