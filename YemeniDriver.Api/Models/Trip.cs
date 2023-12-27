namespace YemeniDriver.Api.Models
{
    public class Trip
    {
        public string TripId { get; set; }
        public string RequestId { get; set; }
        public string DriverId { get; set; }
        public string PassengerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickupLocation { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public int DriverRating { get; set; }
        public int PassengerRating { get; set; }
        public string Comment { get; set; }

        public virtual Request Request { get; set; }
        public virtual ApplicationUser Driver { get; set; }
        public virtual ApplicationUser Passenger { get; set; }

        public virtual ICollection<DriverRating> DriverRatings { get; set; }
        public virtual ICollection<PassengerRating> PassengerRatings { get; set; }
        public virtual ICollection<DriverReview> DriverReviews { get; set; }
        public virtual ICollection<PassengerReview> PassengerReviews { get; set; }
    }
}
