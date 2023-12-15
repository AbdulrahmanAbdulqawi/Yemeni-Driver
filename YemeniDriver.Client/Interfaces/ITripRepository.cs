using YemeniDriver.Models;

namespace Yemeni_Driver.Interfaces
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetAll();
        Task<Trip> GetByIdAsync(string id);
        Task<Trip> GetByIdAsyncNoTracking(string id);
        bool Add(Trip trip);
        bool Update(Trip trip);
        bool Delete(Trip trip);
        bool Save();
    }
}
