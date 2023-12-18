using Microsoft.EntityFrameworkCore;
using YemeniDriver.Data;
using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetAll();
        Task<Trip> GetByIdAsync(string id);
        Task<Trip> GetByIdAsyncNoTracking(string id);
        Task<IEnumerable<Trip>> GetByUserId(string userId, Roles role);
        public bool DeleteRange(List<Trip> requests);
        bool Add(Trip trip);
        bool Update(Trip trip);
        bool Delete(Trip trip);
        bool Save();
    }
}
