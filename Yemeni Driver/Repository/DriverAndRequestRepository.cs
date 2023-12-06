using Microsoft.EntityFrameworkCore;
using Yemeni_Driver.Data;
using Yemeni_Driver.Interfaces;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.Repository
{
    public class DriverAndRequestRepository : IDriverAndRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public DriverAndRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(DriverAndRequest driverAndRequest)
        {
            _context.DriversAndRequests.Add(driverAndRequest);
            return Save();
        }

        public bool Delete(DriverAndRequest driverAndRequest)
        {
            _context.DriversAndRequests.Remove(driverAndRequest);
            return Save();
        }

        public async Task<IEnumerable<DriverAndRequest>> GetDriverAndRequestAsync()
        {
            return await _context.DriversAndRequests.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(DriverAndRequest driverAndRequest)
        {
            _context.DriversAndRequests.Update(driverAndRequest);
            return Save();
        }
    }
}
