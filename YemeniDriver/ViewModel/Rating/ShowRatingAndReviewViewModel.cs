using YemeniDriver.Models;

namespace YemeniDriver.ViewModel.Rating
{
    public class ShowRatingAndReviewViewModel
    {
        public ApplicationUser? Driver { get; set; }
        public Models.Trip? Trip { get; set; }
        public int? RatingValue {  get; set; }
        public string? Comment {  get; set; }
        public string? DriverID { get; set; }
        public string? TripId { get; set; }
    }
}
