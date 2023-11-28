using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yemeni_Driver.Models;

namespace Yemeni_Driver.ViewModel.Vehicle
{
    public class CreateVehicleViewModel
    {
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
    }
}
