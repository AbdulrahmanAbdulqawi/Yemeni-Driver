using Microsoft.EntityFrameworkCore;
using YemeniDriver.Api.Data;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.Interfaces;
using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.Repository
{
    /// <summary>
    /// Repository for handling requests in the application.
    /// </summary>
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public bool Add(Request request)
        {
            _dbContext.Requests.Add(request);
            return Save();
        }

        /// <inheritdoc/>
        public bool Delete(Request request)
        {
            _dbContext.Requests.Remove(request);
            return Save();
        }

        /// <inheritdoc/>
        public bool DeleteRequests(List<Request> requests)
        {
            _dbContext.Requests.RemoveRange(requests);
            return Save();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Request>> GetAll()
        {
            return await _dbContext.Requests.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Request> GetByIdAsync(string id)
        {
            return await _dbContext.Requests.FirstOrDefaultAsync(a => a.RequestId == id);
        }

        /// <inheritdoc/>
        public async Task<Request> GetByIdAsyncNoTracking(string id)
        {
            return await _dbContext.Requests.AsNoTracking().FirstOrDefaultAsync(a => a.RequestId == id);
        }

        /// <inheritdoc/>
        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0;
        }

        /// <inheritdoc/>
        public bool Update(Request request)
        {
            _dbContext.Update(request);
            return Save();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Request>> GetByStatus(RequestStatus requestStatus)
        {
            var requests = await _dbContext.Requests.Where(a => a.Status == requestStatus).ToListAsync();
            return requests;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Request>> GetByUserId(string userId, Roles role)
        {
            IQueryable<Request> query = _dbContext.Requests.Where(a => a.DriverID == userId || a.PassengerId == userId);

            if (role == Roles.Driver)
            {
                query = query.Where(a => a.DriverID == userId);
            }
            else
            {
                query = query.Where(a => a.PassengerId == userId);
            }

            var requests = await query.ToListAsync();

            return requests.Count == 0 ? null : requests;
        }
    }
}
