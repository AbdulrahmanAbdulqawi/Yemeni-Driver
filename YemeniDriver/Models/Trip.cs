using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YemeniDriver.Models
{
    public class Trip
    {
        [Key]
        public string TripId { get; set; }
        [ForeignKey("Request")]
        public string RequestId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration {  get; set; }
        public double Price { get; set; }
        [Range(0,5)]
        public int DriverRating { get; set; }
        [Range(0, 5)]
        public int PassengerRating { get; set; }
        public string Comment { get; set; }


        public virtual Request Request { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
