using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public VehicleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool Add(Vehicle vehicle)
        {
            _applicationDbContext.Vehicles.Add(vehicle);
            return Save();
        }

        public bool Delete(Vehicle vehicle)
        {
            _applicationDbContext.Remove(vehicle);
            return Save();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(string vehicleId)
        {
            return await _applicationDbContext.Vehicles.AsNoTracking().FirstOrDefaultAsync(a => a.VehicleId == vehicleId);
        }

        public async Task<Vehicle> GetVehicleByOwner(string ownerId)
        {
            return await _applicationDbContext.Vehicles.Include(a => a.ApplicationUser).AsNoTracking().FirstOrDefaultAsync(a => a.ApplicationUserId == ownerId);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            return await _applicationDbContext.Vehicles.ToListAsync();
        }

        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 1 ? true : false;
        }

        public bool Update(Vehicle vehicle)
        {
            _applicationDbContext.Update(vehicle);
            return Save();
        }
    }
}
