using Microsoft.EntityFrameworkCore;
using YemeniDriver.Interfaces;
using YemeniDriver.Data;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{

    /// <summary>
    /// Repository for handling trips in the application.
    /// </summary>
    public class TripRepository : ITripRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TripRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public TripRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public bool Add(Trip trip)
        {
            _dbContext.Add(trip);
            return Save();
        }

        /// <inheritdoc/>
        public bool Delete(Trip trip)
        {
            _dbContext.Remove(trip);
            return Save();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Trip>> GetAll()
        {
            return await _dbContext.Trips.ToListAsync();       
        }

        /// <inheritdoc/>
        public async Task<Trip> GetByIdAsync(string id)
        {
            return await _dbContext.Trips.FirstOrDefaultAsync(a => a.TripId == id);
        }

        /// <inheritdoc/>
        public async Task<Trip> GetByIdAsyncNoTracking(string id)
        {
            return await _dbContext.Trips.AsNoTracking().FirstOrDefaultAsync(a => a.TripId == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Trip>> GetByUserId(string userId, Roles role)
        {
            if (role == Roles.Driver)
            {
                return await _dbContext.Trips.Where(a => a.DriverId == userId).ToListAsync();
            }
            return await _dbContext.Trips.Where(a => a.PassengerId == userId).ToListAsync();
        }

        /// <inheritdoc/>
        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        /// <inheritdoc/>
        public bool Update(Trip trip)
        {
            _dbContext.Trips.Update(trip);
            return Save();
        }

        /// <inheritdoc/>
        public bool DeleteRange(List<Trip> trips)
        {
            _dbContext.Trips.RemoveRange(trips);
            return Save();
        }
    }
}
