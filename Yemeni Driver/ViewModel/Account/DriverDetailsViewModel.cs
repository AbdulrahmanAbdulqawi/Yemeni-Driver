using Yemeni_Driver.ViewModel.Vehicle;

namespace Yemeni_Driver.ViewModel.Account
{
    public class DriverDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber {  get; set; }
        public string Image
        {
            get; set;
        }
        public int Rating { get; set; }
        public ViewVehicleViewModel ViewVehicle { get; set; }
    }
}
