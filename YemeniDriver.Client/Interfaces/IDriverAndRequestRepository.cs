using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    public interface IDriverAndRequestRepository
    {
        Task<IEnumerable<DriverAndRequest>> GetDriverAndRequestAsync();
        Task<string> GetDriverIdByRequestId(string requestId);
        bool Add(DriverAndRequest driverAndRequest);
        bool Update(DriverAndRequest driverAndRequest);
        bool Delete(DriverAndRequest driverAndRequest);
        bool Save();
    }
}
