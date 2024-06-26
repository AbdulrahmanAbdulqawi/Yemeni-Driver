﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using YemeniDriver.Data;
using YemeniDriver.Data.Enums;

namespace YemeniDriver.Models
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
        [ForeignKey("Vehicle")]
        public string? VehicleId { get; set; }
        public Roles? Roles { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual ICollection<DriverAndRequest> DriverAndRequests { get; set; }
        public virtual Trip Trip { get; set; }
        public virtual CancelRequest CancelRequest { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
