using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Interfaces
{
    /// <summary>
    /// Represents a repository for retrieving dashboard-related data.
    /// </summary>
    public interface IDashboardRepository
    {
        /// <summary>
        /// Retrieves a list of driver users.
        /// </summary>
        /// <returns>An asynchronous operation that returns a list of driver users.</returns>
        Task<IEnumerable<ApplicationUser>> GetDrivers();

        /// <summary>
        /// Retrieves a list of passenger users.
        /// </summary>
        /// <returns>An asynchronous operation that returns a list of passenger users.</returns>
        Task<IEnumerable<ApplicationUser>> GetPassengers();

        /// <summary>
        /// Retrieves a driver user by their identifier.
        /// </summary>
        /// <param name="driverId">The identifier of the driver user.</param>
        /// <returns>An asynchronous operation that returns the driver user.</returns>
        Task<ApplicationUser> GetDriverByIdAsync(string driverId);

        /// <summary>
        /// Retrieves a driver user by their identifier without tracking changes.
        /// </summary>
        /// <param name="driverId">The identifier of the driver user.</param>
        /// <returns>An asynchronous operation that returns the driver user without tracking changes.</returns>
        Task<ApplicationUser> GetDriverByIdAsyncNoTracking(string driverId);

        /// <summary>
        /// Retrieves a passenger user by their identifier.
        /// </summary>
        /// <param name="passengerId">The identifier of the passenger user.</param>
        /// <returns>An asynchronous operation that returns the passenger user.</returns>
        Task<ApplicationUser> GetPassengerByIdAsync(string passengerId);

        /// <summary>
        /// Retrieves a passenger user by their identifier without tracking changes.
        /// </summary>
        /// <param name="passengerId">The identifier of the passenger user.</param>
        /// <returns>An asynchronous operation that returns the passenger user without tracking changes.</returns>
        Task<ApplicationUser> GetPassengerByIdAsyncNoTracking(string passengerId);
    }
}
