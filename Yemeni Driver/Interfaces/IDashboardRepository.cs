using Yemeni_Driver.Models;

namespace Yemeni_Driver.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<ApplicationUser>> GetDrivers();
        Task<IEnumerable<ApplicationUser>> GetPassengers();


        Task<ApplicationUser> GetDriverByIdAsync(string driverId);
        Task<ApplicationUser> GetDriverByIdAsyncNoTracking(string driverId);

        Task<ApplicationUser> GetPassengerByIdAsync(string passengerId);
        Task<ApplicationUser> GetPassengerByIdAsyncNoTracking(string passengerId);

    }
}
