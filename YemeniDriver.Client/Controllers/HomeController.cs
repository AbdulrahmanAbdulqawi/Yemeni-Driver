using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using YemeniDriver.Models;
using YemeniDriver.ViewModel.Home;

namespace YemeniDriver.Controllers
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
