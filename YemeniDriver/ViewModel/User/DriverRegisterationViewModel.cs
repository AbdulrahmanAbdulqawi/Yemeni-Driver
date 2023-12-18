using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using YemeniDriver.Data.Enums;

namespace YemeniDriver.ViewModel.User
{
    public class DriverRegisterationViewModel : RegisterationBaseViewModel
    {

        [Required]
        public string DrivingLicenseNumber { get; set; }

        [Required]
        public string VehicleId { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        public IFormFile VehicleImage { get; set; }
    }
}
