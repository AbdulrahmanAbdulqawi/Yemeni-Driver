using System.ComponentModel.DataAnnotations.Schema;

namespace Yemeni_Driver.Models
{
    public class CancelRequest
    {

        [ForeignKey("Request")]
        public string? RequestId { get; set; }
        [ForeignKey("Passenger")]
        public string? UserId { get; set; }
        [ForeignKey("Driver")]
        public string? DriverId { get; set; }
        public int Penalty { get; set; }
        public DateTime CancelTime { get; set; }
        public string CancelledBy { get; set; } //either passengerId or DriverId
        public string Reason { get; set; }

        public virtual Request Request { get; set; }
        public virtual Passenger Passenger { get; set; }
        public virtual Driver Driver { get; set; }

    }
}
