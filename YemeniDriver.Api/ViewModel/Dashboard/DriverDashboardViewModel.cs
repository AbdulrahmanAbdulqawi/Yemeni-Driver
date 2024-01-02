using YemeniDriver.Api.Models;

namespace YemeniDriver.Api.ViewModel.Dashboard
{

    public class DriverDashboardViewModel
    {
        public IEnumerable<Models.Request>? Requests { get; set; }
        public IEnumerable<ApplicationUser>? Passengers { get; set; }
        public DriverDashboardViewModel(IEnumerable<Models.Request>? requests, IEnumerable<ApplicationUser>? passengers)
        {
            Requests = requests;
            Passengers = passengers;
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }

    }
}
