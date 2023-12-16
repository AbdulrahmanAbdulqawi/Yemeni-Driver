using YemeniDriver.Models;

namespace YemeniDriver.ViewModel.Dashboard
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
