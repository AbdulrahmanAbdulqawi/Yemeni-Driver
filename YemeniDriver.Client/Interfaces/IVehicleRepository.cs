using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(string vehicleId);
        Task<Vehicle> GetVehicleByOwner(string ownerId);
        bool Add(Vehicle vehicle);
        bool Update(Vehicle vehicle);
        bool Delete(Vehicle vehicle);
        bool Save();
    }
}
