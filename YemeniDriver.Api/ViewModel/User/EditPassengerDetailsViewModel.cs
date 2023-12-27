using System.ComponentModel.DataAnnotations;
using YemeniDriver.Api.Data.Enums;

namespace YemeniDriver.Api.ViewModel.User
{
    public class EditPassengerDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public Gender? Gender { get; set; }

        public string? PhoneNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }


        [Required]
        public IFormFile ProfileImage { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}
