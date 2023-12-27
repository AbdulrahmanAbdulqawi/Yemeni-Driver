using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.ViewModel.Dashboard
{
    public class AdminDashboardViewModel
    {
        public List<ApplicationUser> Drivers { get; set; }
        public List<ApplicationUser> Passengers { get; set; }

    }
}
