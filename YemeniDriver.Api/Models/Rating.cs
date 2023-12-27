namespace YemeniDriver.Api.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public string TripId { get; set; }
        public virtual Trip Trip { get; set; }

    }

    public class DriverRating : Rating
    {
        public string DriverId { get; set; } // User giving the rating

    }

    public class PassengerRating : Rating
    {
        public string PassengerId { get; set; } // User giving the rating

    }
}
