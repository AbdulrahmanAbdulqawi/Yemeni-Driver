using YemeniDriver.Models;

namespace YemeniDriver.Interfaces
{
    public interface IRatingReposiotry
    {
        bool Add(Rating rating);
        bool Update(Rating rating);
        bool Delete(Rating rating);
        bool DeleteRange(List<Rating> ratings);
        bool Save();
    }

    public interface IDriverRatingReposiotry : IRatingReposiotry
    {
        Task<IEnumerable<DriverRating>> GetRatingsByDriverId(string driverId);
        Task GetDriverRatingByTripId(string tripId);

    }

    public interface IPassengerRatingReposiotry : IRatingReposiotry
    {
        Task<IEnumerable<PassengerRating>> GetRatingsByPassengerId(string passengerId);
        Task GetPassengerRatingByTripId(string tripId);

    }
}
