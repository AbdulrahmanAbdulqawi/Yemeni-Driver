using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Yemeni_Driver.Data.Enums;

namespace Yemeni_Driver.ViewModel.Account
{
    public class DriverRegisterationViewModel
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
        [Required]
        public string DrivingLicenseNumber { get; set; }


        [Required]
        public IFormFile ProfileImage { get; set; }
        [Required]
        public string VehicleId { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        public IFormFile VehicleImage { get; set; }
    }
}
