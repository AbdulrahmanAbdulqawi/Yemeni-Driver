using System.ComponentModel.DataAnnotations;

namespace YemeniDriver.Api.ViewModel.Vehicle
{
    public class UpdateVehicleViewModel
    {
        public string VehicleId { get; set; }
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

        public string? UserId { get; set; } // To associat
        public IFormFile VehicleImage { get; set; }
        public string? VehicleImageUrl { get; set; }
    }
}
