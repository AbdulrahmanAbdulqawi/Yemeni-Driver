using YemeniDriver.Data.Enums;
using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAll();
        Task<Request> GetByIdAsync(string id);
        Task<Request> GetByIdAsyncNoTracking(string id);
        Task<IEnumerable<Request>> GetByStatus(RequestStatus requestStatus);
        bool Add(Request request);
        bool Update(Request request);
        bool Delete(Request request);
        bool Save();
    }
}
