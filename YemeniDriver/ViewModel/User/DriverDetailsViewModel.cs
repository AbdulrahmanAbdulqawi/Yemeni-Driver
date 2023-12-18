using YemeniDriver.ViewModel.Vehicle;

namespace YemeniDriver.ViewModel.User
{
    public class DriverDetailsViewModel
    {
        public string? DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image
        {
            get; set;
        }
        public int Rating { get; set; }
        public ViewVehicleViewModel ViewVehicle { get; set; }
    }
}
