using Yemeni_Driver.Models;

namespace Yemeni_Driver.Interfaces
{
    public interface IDriverAndRequestRepository
    {
        Task<IEnumerable<DriverAndRequest>> GetDriverAndRequestAsync();
        bool Add(DriverAndRequest driverAndRequest);
        bool Update(DriverAndRequest driverAndRequest);
        bool Delete(DriverAndRequest driverAndRequest);
        bool Save();
    }
}
