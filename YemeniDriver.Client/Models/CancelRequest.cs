using System.ComponentModel.DataAnnotations.Schema;

namespace YemeniDriver.Models
{
    public class CancelRequest
    {

        [ForeignKey("Request")]
        public string? RequestId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public int Penalty { get; set; }
        public DateTime CancelTime { get; set; }
        public string CancelledBy { get; set; } //either passengerId or DriverId
        public string Reason { get; set; }

        public virtual Request Request { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
