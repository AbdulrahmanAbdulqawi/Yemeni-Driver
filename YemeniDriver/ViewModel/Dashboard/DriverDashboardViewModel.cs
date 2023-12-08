using YemeniDriver.Models;

namespace YemeniDriver.ViewModel.Dashboard
{

    public class DriverDashboardViewModel
    {
        public IEnumerable<Models.Request> Requests { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public DriverDashboardViewModel(IEnumerable<Models.Request> requests, IEnumerable<ApplicationUser> users)
        {
            Requests = requests;
            Users = users;
        }
        public string Id {  get; set; }
        public string FirstName { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }

    }
}
