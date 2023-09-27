using BLL.Services.HRs;
using DAL.Models;
using DMSTaskMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DMSTaskMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ServiceHR _serviceHR;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public HomeController(ILogger<HomeController> logger,
            ServiceHR serviceHR,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _logger = logger;
            _serviceHR = serviceHR;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Employees = _serviceHR.GetEmployeesAsQuerable().Count();
            ViewBag.Users = _userManager.Users.Count();
            ViewBag.Roles = _roleManager.Roles.Count();

            return View();
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