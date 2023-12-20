using Microsoft.EntityFrameworkCore;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;
using YemeniDriver.Data;

namespace YemeniDriver.Repository
{
    public class RatingRepository : IDriverRatingReposiotry, IPassengerRatingReposiotry
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Rating rating)
        {
            if (rating.GetType().Name == typeof(DriverRating).Name)
            {
                _context.DriverRatings.Add((DriverRating)rating);
            }
            else
            {
                _context.PassengerRatings.Add((PassengerRating)rating);
            }
            return Save();
        }

        public bool Delete(Rating rating)
        {
            if (rating.GetType().Name == "DriverRating")
            {
                _context.DriverRatings.Remove((DriverRating)rating);
            }
            else
            {
                _context.PassengerRatings.Remove((PassengerRating)rating);
            }
            return Save();
        }

        public bool DeleteRange(List<Rating> ratings)
        {
            if (ratings.GetType().Name == "IEnumerable<DriverRating>")
            {
                _context.DriverRatings.RemoveRange((IEnumerable<DriverRating>)ratings);
            }
            else
            {
                _context.PassengerRatings.RemoveRange((IEnumerable<PassengerRating>)ratings);
            }
            return Save();
        }

        public async Task GetDriverRatingByTripId(string tripId)
        {
            await _context.DriverRatings.AsNoTracking().FirstOrDefaultAsync(a => a.TripId == tripId);
        }

        public async Task GetPassengerRatingByTripId(string tripId)
        {
            await _context.PassengerRatings.AsNoTracking().FirstOrDefaultAsync(a => a.TripId == tripId);
        }

        public async Task<IEnumerable<DriverRating>> GetRatingsByDriverId(string driverId)
        {
            var driverRatings = await _context.DriverRatings.Where(a => a.DriverId == driverId).ToListAsync();

            return driverRatings;
        }

        public async Task<IEnumerable<PassengerRating>> GetRatingsByPassengerId(string passengerId)
        {
            return (IEnumerable<PassengerRating>)await _context.PassengerRatings.Select(a => a.PassengerId == passengerId).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Rating rating)
        {
            if (rating.GetType().Name == "DriverRating")
            {
                _context.DriverRatings.Update((DriverRating)rating);
            }
            else
            {
                _context.PassengerRatings.Update((PassengerRating)rating);
            }
            return Save();
        }
    }
}
