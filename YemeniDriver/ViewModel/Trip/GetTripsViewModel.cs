using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YemeniDriver.ViewModel.Trip
{
    public class GetTripsViewModel
    {
        public string RequestId { get; set; }
        public string ApplicationUserId { get; set; }
        public string DriverName { get; set; }
        public string PassengerName {  get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickupLocation { get; set; }  
        public int Duration { get; set; }
        public double Price { get; set; }
        public int DriverRating { get; set; }
        public int PassengerRating { get; set; }
        public string Comment { get; set; }
    }
}
