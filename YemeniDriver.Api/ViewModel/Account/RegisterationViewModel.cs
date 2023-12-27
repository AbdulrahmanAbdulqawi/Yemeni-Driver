using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using YemeniDriver.Api.Data.Enums;

namespace YemeniDriver.Api.ViewModel.Account
{
    public class RegisterationViewModel
    {
        [NotNull]
        public string FirstName { get; set; }
        [NotNull]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Length(8, 20)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [NotNull]
        public Gender Gender { get; set; }
        [NotNull]
        [Length(6, 10)]
        public string PhoneNumber { get; set; }
        [Range(0, 5)]
        public int? Rateing { get; set; }
        public Roles Roles { get; set; } = Roles.Driver;
        public string DrivingLicenseNumber { get; set; }
        public double? InitialLatitude { get; set; }
        public double? InitialLongitude { get; set; }
        public string? VehicleId { get; set; }

        public string? Model { get; set; }

        public string? Make { get; set; }

        public int? Year { get; set; }

        public int? Capacity { get; set; }

        public string? Color { get; set; }

        public string? PlateNumber { get; set; }
        public IFormFile? VehicleImage { get; set; }
        [Required]
        public IFormFile ProfileImage { get; set; }

    }
}
