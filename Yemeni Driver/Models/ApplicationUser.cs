using Microsoft.AspNetCore.Identity;
using Yemeni_Driver.Data;
using Yemeni_Driver.Data.Enums;

namespace Yemeni_Driver.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Rating { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public Roles? Roles { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<DriverAndRequest> DriverAndRequests { get; set; }
        public virtual Trip Trip { get; set; }
        public virtual CancelRequest CancelRequest { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
