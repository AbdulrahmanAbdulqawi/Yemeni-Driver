using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using YemeniDriver.Models;
using YemeniDriver.ViewModel.Home;

namespace YemeniDriver.Controllers
{
    /// <summary>
    /// Controller responsible for handling home-related actions and views.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Constructor for the HomeController.
        /// </summary>
        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Action to display the home page.
        /// </summary>
        public IActionResult Index()
        {
            try
            {
                var model = new RegisterSelectRolesViewModel
                {
                    AvailableRoles = _roleManager.Roles
                        .Where(a => a.NormalizedName != "Admin")
                        .Select(a => new SelectListItem { Text = a.NormalizedName, Value = a.Name })
                        .ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available roles for the home page.");
                return View("Error");
            }
        }

        /// <summary>
        /// Action to display the privacy page.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Action to handle errors and display the error page.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
