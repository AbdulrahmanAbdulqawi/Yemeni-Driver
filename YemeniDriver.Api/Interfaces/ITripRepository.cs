using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Interfaces
{
    /// <summary>
    /// Represents a repository for handling trips in the application.
    /// </summary>
    public interface ITripRepository
    {
        /// <summary>
        /// Gets all trips in the repository.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of trips.</returns>
        Task<IEnumerable<Trip>> GetAll();

        /// <summary>
        /// Gets a trip by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the trip.</param>
        /// <returns>An asynchronous operation that returns the requested trip.</returns>
        Task<Trip> GetByIdAsync(string id);

        /// <summary>
        /// Gets a trip by its unique identifier without tracking changes.
        /// </summary>
        /// <param name="id">The unique identifier of the trip.</param>
        /// <returns>An asynchronous operation that returns the requested trip without tracking changes.</returns>
        Task<Trip> GetByIdAsyncNoTracking(string id);

        /// <summary>
        /// Gets trips by user ID and role.
        /// </summary>
        /// <param name="userId">The user ID associated with the trips.</param>
        /// <param name="role">The role of the user (Driver or Passenger).</param>
        /// <returns>An asynchronous operation that returns a collection of trips associated with the specified user ID and role.</returns>
        Task<IEnumerable<Trip>> GetByUserId(string userId, Roles role);

        /// <summary>
        /// Adds a new trip to the repository.
        /// </summary>
        /// <param name="trip">The trip to be added.</param>
        /// <returns><c>true</c> if the trip is successfully added; otherwise, <c>false</c>.</returns>
        bool Add(Trip trip);

        /// <summary>
        /// Updates an existing trip in the repository.
        /// </summary>
        /// <param name="trip">The trip to be updated.</param>
        /// <returns><c>true</c> if the trip is successfully updated; otherwise, <c>false</c>.</returns>
        bool Update(Trip trip);

        /// <summary>
        /// Deletes a trip from the repository.
        /// </summary>
        /// <param name="trip">The trip to be deleted.</param>
        /// <returns><c>true</c> if the trip is successfully deleted; otherwise, <c>false</c>.</returns>
        bool Delete(Trip trip);

        /// <summary>
        /// Deletes a range of trips from the repository.
        /// </summary>
        /// <param name="trips">The collection of trips to be deleted.</param>
        /// <returns><c>true</c> if the trips are successfully deleted; otherwise, <c>false</c>.</returns>
        bool DeleteRange(List<Trip> trips);

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns><c>true</c> if changes are successfully saved; otherwise, <c>false</c>.</returns>
        bool Save();
    }
}
