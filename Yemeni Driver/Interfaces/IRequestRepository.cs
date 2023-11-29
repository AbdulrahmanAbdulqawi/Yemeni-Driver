using Yemeni_Driver.Models;

namespace Yemeni_Driver.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAll();
        Task<Request> GetByIdAsync(string id);
        Task<Request> GetByIdAsyncNoTracking(string id);
        bool Add(Request request);
        bool Update(Request request);
        bool Delete(Request request);
        bool Save();
    }
}
