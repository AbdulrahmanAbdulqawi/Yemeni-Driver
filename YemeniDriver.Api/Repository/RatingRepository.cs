using Microsoft.EntityFrameworkCore;
using YemeniDriver.Api.Data;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Repository
{
    /// <summary>
    /// Repository for handling driver and passenger ratings.
    /// </summary>
    public class RatingRepository : IDriverRatingReposiotry, IPassengerRatingReposiotry
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for the RatingRepository.
        /// </summary>
        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public bool Add(Rating rating)
        {
            try
            {
                if (rating is DriverRating)
                {
                    _context.DriverRatings.Add((DriverRating)rating);
                }
                else if (rating is PassengerRating)
                {
                    _context.PassengerRatings.Add((PassengerRating)rating);
                }
                else
                {
                    throw new ArgumentException("Invalid rating type");
                }

                return Save();
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error adding rating to the database");
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Delete(Rating rating)
        {
            try
            {
                if (rating is DriverRating)
                {
                    _context.DriverRatings.Remove((DriverRating)rating);
                }
                else if (rating is PassengerRating)
                {
                    _context.PassengerRatings.Remove((PassengerRating)rating);
                }
                else
                {
                    throw new ArgumentException("Invalid rating type");
                }

                return Save();
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error deleting rating from the database");
                return false;
            }
        }

        /// <inheritdoc/>
        public bool DeleteRatings(List<Rating> ratings)
        {
            try
            {
                if (ratings.All(r => r is DriverRating))
                {
                    _context.DriverRatings.RemoveRange(ratings.Cast<DriverRating>());
                }
                else if (ratings.All(r => r is PassengerRating))
                {
                    _context.PassengerRatings.RemoveRange(ratings.Cast<PassengerRating>());
                }
                else
                {
                    throw new ArgumentException("Invalid rating type in the list");
                }

                return Save();
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error deleting range of ratings from the database");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<DriverRating> GetDriverRatingByTripId(string tripId)
        {
            try
            {
                return await _context.DriverRatings.AsNoTracking().FirstOrDefaultAsync(a => a.TripId == tripId);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error getting driver rating by trip ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<PassengerRating> GetPassengerRatingByTripId(string tripId)
        {
            try
            {
                return await _context.PassengerRatings.AsNoTracking().FirstOrDefaultAsync(a => a.TripId == tripId);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error getting passenger rating by trip ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DriverRating>> GetRatingsByDriverId(string driverId)
        {
            try
            {
                return await _context.DriverRatings.Where(a => a.DriverId == driverId).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error getting driver ratings by driver ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PassengerRating>> GetRatingsByPassengerId(string passengerId)
        {
            try
            {
                return await _context.PassengerRatings.Where(a => a.PassengerId == passengerId).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error getting passenger ratings by passenger ID");
                throw;
            }
        }

        /// <inheritdoc/>
        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error saving changes to the database");
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Update(Rating rating)
        {
            try
            {
                if (rating is DriverRating)
                {
                    _context.DriverRatings.Update((DriverRating)rating);
                }
                else if (rating is PassengerRating)
                {
                    _context.PassengerRatings.Update((PassengerRating)rating);
                }
                else
                {
                    throw new ArgumentException("Invalid rating type");
                }

                return Save();
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "Error updating rating in the database");
                return false;
            }
        }



    }
}
