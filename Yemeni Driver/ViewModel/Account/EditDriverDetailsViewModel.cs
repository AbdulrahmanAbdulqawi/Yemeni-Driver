using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Yemeni_Driver.Data.Enums;
using Yemeni_Driver.Models;
using Yemeni_Driver.ViewModel.Vehicle;

namespace Yemeni_Driver.ViewModel.Account
{
    public class EditDriverDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        //public string Password { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

        public Gender? Gender { get; set; }

        public string? PhoneNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }


        [Required]
        public IFormFile ProfileImage { get; set; }

        public string? ProfileImageUrl { get; set; }     
        public Models.Vehicle Vehicle { get; set; }
        public IFormFile VehicleImage { get; set; }
    }
}
