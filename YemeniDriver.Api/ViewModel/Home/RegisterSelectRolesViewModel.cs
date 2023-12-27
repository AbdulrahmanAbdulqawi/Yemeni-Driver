using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace YemeniDriver.Api.ViewModel.Home
{
    public class RegisterSelectRolesViewModel
    {
        [Required(ErrorMessage = "Please select a role.")]
        public string SelectedRole { get; set; }

        public List<SelectListItem> AvailableRoles { get; set; }
    }
}
