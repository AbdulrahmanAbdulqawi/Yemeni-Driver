using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.ViewModel.Dashboard
{
    public class SharedLayoutViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public SharedLayoutViewModel(IEnumerable<ApplicationUser> users)
        {
            Users = users;
        }

    }
}
