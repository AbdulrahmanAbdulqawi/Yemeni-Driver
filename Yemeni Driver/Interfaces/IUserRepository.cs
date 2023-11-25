using Yemeni_Driver.Models;

namespace Yemeni_Driver.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByIdAsyncNoTracking(string id);
        bool Add(ApplicationUser user);
        bool Update(ApplicationUser user);
        bool Delete(ApplicationUser user);
        bool Save();
    }
}
