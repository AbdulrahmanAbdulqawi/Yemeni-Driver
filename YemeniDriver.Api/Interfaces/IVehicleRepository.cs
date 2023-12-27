using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Interfaces
{
    /// <summary>
    /// Represents a repository for handling vehicle-related operations in the application.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Gets all vehicles in the repository.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();

        /// <summary>
        /// Gets a vehicle by its unique identifier asynchronously.
        /// </summary>
        /// <param name="vehicleId">The unique identifier of the vehicle.</param>
        /// <returns>An asynchronous operation that returns the requested vehicle.</returns>
        Task<Vehicle> GetVehicleByIdAsync(string vehicleId);

        /// <summary>
        /// Gets a vehicle by its owner's unique identifier asynchronously.
        /// </summary>
        /// <param name="ownerId">The unique identifier of the owner (user) of the vehicle.</param>
        /// <returns>An asynchronous operation that returns the requested vehicle.</returns>
        Task<Vehicle> GetVehicleByOwner(string ownerId);

        /// <summary>
        /// Adds a new vehicle to the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle to be added.</param>
        /// <returns><c>true</c> if the vehicle is successfully added; otherwise, <c>false</c>.</returns>
        bool Add(Vehicle vehicle);

        /// <summary>
        /// Updates an existing vehicle in the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle to be updated.</param>
        /// <returns><c>true</c> if the vehicle is successfully updated; otherwise, <c>false</c>.</returns>
        bool Update(Vehicle vehicle);

        /// <summary>
        /// Deletes a vehicle from the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle to be deleted.</param>
        /// <returns><c>true</c> if the vehicle is successfully deleted; otherwise, <c>false</c>.</returns>
        bool Delete(Vehicle vehicle);

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns><c>true</c> if changes are successfully saved; otherwise, <c>false</c>.</returns>
        bool Save();
    }
}
