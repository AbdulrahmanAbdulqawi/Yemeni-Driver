using Microsoft.AspNetCore.Identity;
using YemeniDriver.Api.Data.Enums;

namespace YemeniDriver.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Rating { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public double? LiveLocationLatitude { get; set; }
        public double? LiveLocationLongitude { get; set; }
        public string? Location { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? VehicleId { get; set; }
        public Roles? Roles { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual ICollection<Trip> DriverTrips { get; set; }
        public virtual ICollection<Trip> PassengerTrips { get; set; }


        //public virtual CancelRequest CancelRequest { get; set; }
        public virtual ICollection<Request> DriverRequests { get; set; }
        public virtual ICollection<Request> PassengerRequests { get; set; }
    }
}
