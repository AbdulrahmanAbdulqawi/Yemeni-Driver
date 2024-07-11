using System.ComponentModel.DataAnnotations;
using YemeniDriver.Api.Data.Enums;
using YemeniDriver.Api.ViewModel.Vehicle;

namespace YemeniDriver.Api.ViewModel.User
{
    public class EditDriverDetailsViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }

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
        public ViewVehicleViewModel? Vehicle { get; set; }
        public IFormFile VehicleImage { get; set; }
    }
}
