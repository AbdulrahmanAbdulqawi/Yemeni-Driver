using YemeniDriver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YemeniDriver.Interfaces
{
    /// <summary>
    /// Represents a repository for managing ratings in the YemeniDriver application.
    /// </summary>
    public interface IRatingReposiotry
    {
        /// <summary>
        /// Adds a new rating to the repository.
        /// </summary>
        /// <param name="rating">The rating to be added.</param>
        /// <returns>True if the rating is successfully added; otherwise, false.</returns>
        bool Add(Rating rating);

        /// <summary>
        /// Updates an existing rating in the repository.
        /// </summary>
        /// <param name="rating">The rating to be updated.</param>
        /// <returns>True if the rating is successfully updated; otherwise, false.</returns>
        bool Update(Rating rating);

        /// <summary>
        /// Deletes a specific rating from the repository.
        /// </summary>
        /// <param name="rating">The rating to be deleted.</param>
        /// <returns>True if the rating is successfully deleted; otherwise, false.</returns>
        bool Delete(Rating rating);

        /// <summary>
        /// Deletes a range of ratings from the repository.
        /// </summary>
        /// <param name="ratings">The list of ratings to be deleted.</param>
        /// <returns>True if the ratings are successfully deleted; otherwise, false.</returns>
        bool DeleteRange(List<Rating> ratings);

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns>True if changes are successfully saved; otherwise, false.</returns>
        bool Save();
    }

    /// <summary>
    /// Represents a repository for managing driver ratings in the YemeniDriver application.
    /// </summary>
    public interface IDriverRatingReposiotry : IRatingReposiotry
    {
        /// <summary>
        /// Retrieves all ratings associated with a specific driver.
        /// </summary>
        /// <param name="driverId">The unique identifier of the driver.</param>
        /// <returns>An asynchronous task that represents the operation and contains a collection of driver ratings.</returns>
        Task<IEnumerable<DriverRating>> GetRatingsByDriverId(string driverId);

        /// <summary>
        /// Retrieves the driver rating associated with a specific trip.
        /// </summary>
        /// <param name="tripId">The unique identifier of the trip.</param>
        /// <returns>An asynchronous task that represents the operation and contains the driver rating.</returns>
        Task<DriverRating> GetDriverRatingByTripId(string tripId);
    }

    /// <summary>
    /// Represents a repository for managing passenger ratings in the YemeniDriver application.
    /// </summary>
    public interface IPassengerRatingReposiotry : IRatingReposiotry
    {
        /// <summary>
        /// Retrieves all ratings associated with a specific passenger.
        /// </summary>
        /// <param name="passengerId">The unique identifier of the passenger.</param>
        /// <returns>An asynchronous task that represents the operation and contains a collection of passenger ratings.</returns>
        Task<IEnumerable<PassengerRating>> GetRatingsByPassengerId(string passengerId);

        /// <summary>
        /// Retrieves the passenger rating associated with a specific trip.
        /// </summary>
        /// <param name="tripId">The unique identifier of the trip.</param>
        /// <returns>An asynchronous task that represents the operation and contains the passenger rating.</returns>
        Task<PassengerRating> GetPassengerRatingByTripId(string tripId);
    }
}
