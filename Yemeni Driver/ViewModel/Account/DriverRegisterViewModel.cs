using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Yemeni_Driver.Data.Enums;
using Yemeni_Driver.Data;

namespace Yemeni_Driver.ViewModel.Account
{
    public class DriverRegisterViewModel
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
    }
}
