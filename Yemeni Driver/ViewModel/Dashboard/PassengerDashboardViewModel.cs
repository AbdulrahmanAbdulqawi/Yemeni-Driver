using Yemeni_Driver.Models;

namespace Yemeni_Driver.ViewModel.Dashboard
{
    public class PassengerDashboardViewModel
    {
        public List<ApplicationUser> AvailableDrivers { get; set; }
        // Add other properties as needed for the passenger dashboard

        public PassengerDashboardViewModel(List<ApplicationUser> availableDrivers)
        {
            AvailableDrivers = availableDrivers;
        }
        public string FirstName { get; set; }
        public string Location { get; set; }
    }
}
