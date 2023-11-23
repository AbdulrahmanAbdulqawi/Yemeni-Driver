using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yemeni_Driver.Models
{
    public class Driver : User
    {
        [Key]
        public string DriverId { get; set; }
        [Length(8,8)]
        public string DrivingLicenceNumber { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<DriverAndRequest> DriverAndRequests { get; set; }
        public virtual Trip Trip { get; set; }
        public virtual CancelRequest CancelRequest { get; set; }


    }
}
