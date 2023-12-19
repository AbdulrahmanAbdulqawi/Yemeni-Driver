using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YemeniDriver.Data.Enums;

namespace YemeniDriver.Models
{
    public class Request
    {
        [Key]
        public string RequestId { get; set; }
        public string PassengerId { get; set; }
        public string DriverID { get; set; }
        public string? TripId { get; set; }
        public DateTime PickupTime { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public double EstimationPrice { get; set; }
        public int NumberOfSeats { get; set; }
        public RequestStatus Status { get; set; }

        public virtual ApplicationUser Driver { get; set; }
        public virtual ApplicationUser Passenger { get; set; }
        public virtual Trip Trip { get; set; }
       // public virtual CancelRequest CancelRequest { get; set; }



    }
}
