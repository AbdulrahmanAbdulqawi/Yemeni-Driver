using Microsoft.EntityFrameworkCore;
using YemeniDriver.Data;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    public class DriverAndRequestRepository 
    {
        private readonly ApplicationDbContext _context;
        public DriverAndRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public bool Add(DriverAndRequest driverAndRequest)
        //{
        //    _context.DriversAndRequests.Add(driverAndRequest);
        //    return Save();
        //}

        //public bool Delete(DriverAndRequest driverAndRequest)
        //{
        //    _context.DriversAndRequests.Remove(driverAndRequest);
        //    return Save();
        //}

        //public async Task<IEnumerable<DriverAndRequest>> GetDriverAndRequestAsync()
        //{
        //    return await _context.DriversAndRequests.ToListAsync();
        //}

        //public async Task<string> GetDriverIdByRequestId(string requestId)
        //{
        //    var driver = await _context.DriversAndRequests.FindAsync(requestId);
        //    if(driver == null)
        //    {
        //        return null;
        //    }
        //    return await Task.FromResult(driver.DriverId);
        //}

        //public bool Save()
        //{
        //    var saved = _context.SaveChanges();
        //    return saved > 0 ? true : false;
        //}

        //public bool Update(DriverAndRequest driverAndRequest)
        //{
        //    _context.DriversAndRequests.Update(driverAndRequest);
        //    return Save();
        //}
    }
}
