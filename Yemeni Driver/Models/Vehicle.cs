using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yemeni_Driver.Models
{
    public class Vehicle
    {
        [Key]
        public string? VehicleId { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public int? Year { get; set; }
        
        public int? Capacity { get; set; }
        public string? Color { get; set; }
        public string? PlateNumber { get; set; }
        public string? VehiclImageUrl { get; set; }
        public string? ApplicationUserId {  get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
