namespace YemeniDriver.Api.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public string TripId { get; set; }

        // Navigation property for the trip
        public virtual Trip Trip { get; set; }
    }

    public class DriverReview : Review
    {
        public string DriverId { get; set; } // User giving the rating
    }
    public class PassengerReview : Review
    {
        public string PassengerId { get; set; } // User giving the rating
    }
}
