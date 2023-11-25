using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Yemeni_Driver.Data;

namespace Yemeni_Driver.ViewModel.Home
{
    public class RegisterSelectRolesViewModel
    {
        [Required(ErrorMessage = "Please select a role.")]
        public string SelectedRole { get; set; }

        public List<SelectListItem> AvailableRoles { get; set; }
    }
}
