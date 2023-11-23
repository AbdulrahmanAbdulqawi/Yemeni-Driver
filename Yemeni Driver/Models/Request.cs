using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yemeni_Driver.Data.Enums;

namespace Yemeni_Driver.Models
{
    public class Request
    {
        [Key]
        public string RequestId { get; set; }
        [ForeignKey("Passenger")]
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public RideType RideType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public double EstimationPrice { get; set; }
        public int NumberOfSeats { get; set; }

        public virtual Passenger Passenger { get; set; }
        public virtual ICollection<DriverAndRequest> DriverAndRequests { get; set; }
        public virtual Trip Trip { get; set; }
        public virtual CancelRequest CancelRequest { get; set; }

    }
}
