using Microsoft.EntityFrameworkCore;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    /// <summary>
    /// Repository for handling Vehicles.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public VehicleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <inheritdoc/>
        public bool Add(Vehicle vehicle)
        {
            _applicationDbContext.Vehicles.Add(vehicle);
            return Save();
        }

        /// <inheritdoc/>
        public bool Delete(Vehicle vehicle)
        {
            _applicationDbContext.Remove(vehicle);
            return Save();
        }

        /// <inheritdoc/>
        public async Task<Vehicle> GetVehicleByIdAsync(string vehicleId)
        {
            return await _applicationDbContext.Vehicles.AsNoTracking().FirstOrDefaultAsync(a => a.VehicleId == vehicleId);
        }

        /// <inheritdoc/>
        public async Task<Vehicle> GetVehicleByOwner(string ownerId)
        {
            return await _applicationDbContext.Vehicles.AsNoTracking().Include(a => a.Driver).AsNoTracking().FirstOrDefaultAsync(a => a.DriverId == ownerId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            return await _applicationDbContext.Vehicles.ToListAsync();
        }

        /// <inheritdoc/>
        public bool Save()
        {
            var saved = _applicationDbContext.SaveChanges();
            return saved > 1 ? true : false;
        }

        /// <inheritdoc/>
        public bool Update(Vehicle vehicle)
        {
            _applicationDbContext.Update(vehicle);
            return Save();
        }
    }
}
