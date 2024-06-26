﻿using Microsoft.EntityFrameworkCore;
using YemeniDriver.Data;
using YemeniDriver.Data.Enums;
using YemeniDriver.Interfaces;
using YemeniDriver.Models;

namespace YemeniDriver.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Add(Request request)
        {
            _dbContext.Requests.Add(request);
            return Save();
        }

        public bool Delete(Request request)
        {
            _dbContext.Requests.Remove(request);
            return Save();
        }

        public async Task<IEnumerable<Request>> GetAll()
        {
            return await _dbContext.Requests.ToListAsync();
        }

        public async Task<Request> GetByIdAsync(string id)
        {
            return await _dbContext.Requests.FirstOrDefaultAsync(a => a.RequestId == id);
        }

        public async Task<Request> GetByIdAsyncNoTracking(string id)
        {
            return await _dbContext.Requests.AsNoTracking().FirstOrDefaultAsync(a => a.RequestId == id);
        }


        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Request request)
        {
            _dbContext.Update(request);
            return Save();
        }

        public async Task<IEnumerable<Request>> GetByStatus(RequestStatus requestStatus)
        {
            var requests = await _dbContext.Requests.Where(a => a.Status == requestStatus).ToListAsync();

            return requests;
        }
    }
}
