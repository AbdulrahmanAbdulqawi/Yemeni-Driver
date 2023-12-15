using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using YemeniDriver.Data.Enums;

namespace YemeniDriver.ViewModel.Account
{
    public class PassengerRegisterationViewModel
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
        public IFormFile ProfileImage { get; set; }
    }
}
