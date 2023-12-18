using YemeniDriver.Models;

namespace YemeniDriver.ViewModel.Dashboard
{
    public class AdminDashboardViewModel
    {
        public List<ApplicationUser> Drivers { get; set; }
        public List<ApplicationUser> Passengers { get; set; }

    }
}
