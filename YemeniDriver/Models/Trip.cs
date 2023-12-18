using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YemeniDriver.Models
{
    public class Trip
    {
        [Key]
        public string TripId { get; set; }
        public string RequestId { get; set; }
        public string DriverId { get; set; }
        public string PassengerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickupLocation { get; set; }
        public int Duration {  get; set; }
        public double Price { get; set; }
        public int DriverRating { get; set; }
        public int PassengerRating { get; set; }
        public string Comment { get; set; }

        public virtual Request Request { get; set; }
        public virtual ApplicationUser Driver { get; set; }
        public virtual ApplicationUser Passenger { get; set; }


    }
}
