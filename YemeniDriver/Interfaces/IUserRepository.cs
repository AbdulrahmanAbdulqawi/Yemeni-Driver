using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    /// <summary>
    /// Represents a repository for handling user-related operations in the application.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users in the repository.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of users.</returns>
        Task<IEnumerable<ApplicationUser>> GetAll();

        /// <summary>
        /// Gets all drivers in the repository.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of driver users.</returns>
        Task<IEnumerable<ApplicationUser>> GetDrivers();

        /// <summary>
        /// Gets all passengers in the repository.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of passenger users.</returns>
        Task<IEnumerable<ApplicationUser>> GetPassengers();

        /// <summary>
        /// Gets the location of a user based on their user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>An asynchronous operation that returns the location of the user as a tuple of latitude and longitude.</returns>
        Task<List<(double?, double?)>> GetUserLocation(string userId);

        /// <summary>
        /// Gets a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>An asynchronous operation that returns the requested user.</returns>
        Task<ApplicationUser> GetByIdAsync(string id);

        /// <summary>
        /// Gets a user by their unique identifier without tracking changes.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>An asynchronous operation that returns the requested user without tracking changes.</returns>
        Task<ApplicationUser> GetByIdAsyncNoTracking(string id);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <returns><c>true</c> if the user is successfully added; otherwise, <c>false</c>.</returns>
        bool Add(ApplicationUser user);

        /// <summary>
        /// Updates an existing user in the repository.
        /// </summary>
        /// <param name="user">The user to be updated.</param>
        /// <returns><c>true</c> if the user is successfully updated; otherwise, <c>false</c>.</returns>
        bool Update(ApplicationUser user);

        /// <summary>
        /// Deletes a user from the repository.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns><c>true</c> if the user is successfully deleted; otherwise, <c>false</c>.</returns>
        bool Delete(ApplicationUser user);

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns><c>true</c> if changes are successfully saved; otherwise, <c>false</c>.</returns>
        bool Save();
    }
}
