using YemeniDriver.Data;
using YemeniDriver.Data.Enums;
using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    /// <summary>
    /// Represents a repository for handling requests in the application.
    /// </summary>
    public interface IRequestRepository
    {
        /// <summary>
        /// Gets all requests in the repository.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of requests.</returns>
        Task<IEnumerable<Request>> GetAll();

        /// <summary>
        /// Gets a request by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the request.</param>
        /// <returns>An asynchronous operation that returns the requested request.</returns>
        Task<Request> GetByIdAsync(string id);

        /// <summary>
        /// Gets a request by its unique identifier without tracking changes.
        /// </summary>
        /// <param name="id">The unique identifier of the request.</param>
        /// <returns>An asynchronous operation that returns the requested request without tracking changes.</returns>
        Task<Request> GetByIdAsyncNoTracking(string id);

        /// <summary>
        /// Gets requests by their status.
        /// </summary>
        /// <param name="requestStatus">The status of the requests to retrieve.</param>
        /// <returns>An asynchronous operation that returns a collection of requests with the specified status.</returns>
        Task<IEnumerable<Request>> GetByStatus(RequestStatus requestStatus);

        /// <summary>
        /// Gets requests by user ID and role.
        /// </summary>
        /// <param name="userId">The user ID associated with the requests.</param>
        /// <param name="role">The role of the user (Driver or Passenger).</param>
        /// <returns>An asynchronous operation that returns a collection of requests associated with the specified user ID and role.</returns>
        Task<IEnumerable<Request>> GetByUserId(string userId, Roles role);

        /// <summary>
        /// Adds a new request to the repository.
        /// </summary>
        /// <param name="request">The request to be added.</param>
        /// <returns><c>true</c> if the request is successfully added; otherwise, <c>false</c>.</returns>
        bool Add(Request request);

        /// <summary>
        /// Updates an existing request in the repository.
        /// </summary>
        /// <param name="request">The request to be updated.</param>
        /// <returns><c>true</c> if the request is successfully updated; otherwise, <c>false</c>.</returns>
        bool Update(Request request);

        /// <summary>
        /// Deletes a request from the repository.
        /// </summary>
        /// <param name="request">The request to be deleted.</param>
        /// <returns><c>true</c> if the request is successfully deleted; otherwise, <c>false</c>.</returns>
        bool Delete(Request request);

        /// <summary>
        /// Deletes a range of requests from the repository.
        /// </summary>
        /// <param name="requests">The collection of requests to be deleted.</param>
        /// <returns><c>true</c> if the requests are successfully deleted; otherwise, <c>false</c>.</returns>
        bool DeleteRange(List<Request> requests);

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns><c>true</c> if changes are successfully saved; otherwise, <c>false</c>.</returns>
        bool Save();
    }
}
