using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Yemeni_Driver.Models;
using Yemeni_Driver.ViewModel.Home;

namespace Yemeni_Driver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }

  
        
        public IActionResult Index()
        {
            var model = new RegisterSelectRolesViewModel
            {
                AvailableRoles = _roleManager.Roles.Where(a => a.NormalizedName != "Admin").Select(a => new SelectListItem { Text = a.NormalizedName, Value = a.Name }).ToList()
              
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Index(RegisterSelectRolesViewModel model)
        {
            if (model.SelectedRole.Contains("Passenger"))
            {
                return RedirectToAction("RegisterAsPassenger", "Account", new { role = model.SelectedRole });
            }
            // Redirect to the registration page with the selected role
            return RedirectToAction("RegisterAsDriver", "Account", new { role = model.SelectedRole });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
