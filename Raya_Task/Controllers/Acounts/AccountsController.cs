using BLL.DTOs.Users;
using BLL.Extensions.Users;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RayaTaskMVC.Controllers.Acounts
{
    public class AccountsController : Controller
    {
        private readonly UserManager<User> _adminManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountsController(
            UserManager<User> adminManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager)
        {
            _adminManager = adminManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogIN()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIN([FromForm] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);
            try
            {

                var _user = await _adminManager.FindByNameAsync(loginModel.UserName);
                if(_user is null)
                 _user = await _adminManager.FindByEmailAsync(loginModel.UserName);


                if (_user is null || !await _adminManager.CheckPasswordAsync(_user, loginModel.Password))
                {
                    ModelState.AddModelError(string.Empty, "UserName or Password Is Incorrect!");
                    return View(loginModel);

                }
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, true,true);
               
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(_user, loginModel.RememberMe);
                    return RedirectToAction("Index", "HRs");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login");
                    return View(loginModel);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(loginModel);
            }

        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([FromForm] RegistrationModel registrationModel)
        {
            if (!ModelState.IsValid)
                return View(registrationModel);
            try
            {
                if (await _adminManager.FindByEmailAsync(registrationModel.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, "Email Is Already Registered!");
                    return View(registrationModel);
                }

                if (await _adminManager.FindByNameAsync(registrationModel.UserName) != null)
                {
                    ModelState.AddModelError(string.Empty, "UserName Is Already Registered!");
                    return View(registrationModel);
                }
                User _user = registrationModel.ToModel();
                var result = await _adminManager.CreateAsync(_user, registrationModel.Password);
                if (result.Succeeded)
                {
                    await _adminManager.AddToRoleAsync(_user, "HR");
                    //var token = await _adminManager.GenerateEmailConfirmationTokenAsync(_user);
                    //if (!string.IsNullOrEmpty(token))
                    //{
                    //    await SendEmailConfirmationEmail(_user, token);
                    //}
                    //return View(nameof(GOToEmailToConfirm));
                    //return View (registrationModel);

                    return RedirectToAction("Index", "HRs");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        //errors += $"{error.Description},";
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(registrationModel);

                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(registrationModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIN));
        }

    }
}
