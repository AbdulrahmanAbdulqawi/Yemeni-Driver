using System.ComponentModel.DataAnnotations;

namespace Yemeni_Driver.Models
{
    public class Vehicle
    {
        [Key]
        public string VehicleId { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        
        public int Capacity { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }

        public virtual Driver Driver { get; set; }
    }
}
