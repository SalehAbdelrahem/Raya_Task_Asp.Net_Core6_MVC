using AutoMapper;
using BLL.DTOs.HRs;
using BLL.DTOs.Users;
using BLL.Enums.HRs;
using BLL.Services.HRs;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Raya_Task.Controllers.Admin
{
    //[Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        public UsersController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetAllUsers()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);

            var searchValue = Request.Form["search[value]"];

            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];


            IQueryable<UserDTO> users = _userManager.Users.Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName

            })
              .Where(m => string.IsNullOrEmpty(searchValue) ? true :
              m.FirstName.Contains(searchValue) ||
              m.LastName.Contains(searchValue) ||
              m.UserName.Contains(searchValue) ||
              m.Email.Contains(searchValue));

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                users = users.OrderBy(string.Concat(sortColumn, " ", sortColumnDirection));



            var data = users.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = users.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(userDTO);
            }
            try
            {
                ViewBag.Roles = new SelectList(_roleManager.Roles, "Id", "Name");

                if (await _userManager.FindByEmailAsync(userDTO.Email) is not null)
                {
                    ModelState.AddModelError(string.Empty, "Email Is Already Registered!");
                    return View(userDTO);
                }

                if (await _userManager.FindByNameAsync(userDTO.UserName) is not null)
                {
                    ModelState.AddModelError(string.Empty, "UserName Is Already Registered!");
                    return View(userDTO);
                }
                User _user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(_user, userDTO.Password);
                if (result.Succeeded)
                {
                    if (userDTO.RoleIds is not null)
                    {
                        IEnumerable<string> roles = _roleManager.Roles.Where(x => userDTO.RoleIds.Contains(x.Id)).Select(n => n.Name).ToList();
                        var userCreated = await _userManager.FindByEmailAsync(_user.Email);
                        var result2 = await _userManager.AddToRolesAsync(userCreated, roles);
                        if (result2.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result2.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }

                            return View(userDTO);
                        }
                    }



                    return RedirectToAction("Index");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        //errors += $"{error.Description},";
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(userDTO);

                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userDTO);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View(id);
            }
            try
            {
                var result = await _userManager.FindByIdAsync(id.ToString());
                if (result is not null)
                {
                    var result2 = await _userManager.DeleteAsync(result);
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        var errors = new StringBuilder();
                        foreach (var item in result2.Errors)
                        {
                            errors.Append($"{item.Description} , ");
                        }
                        throw new Exception(errors.ToString());

                    }
                }
                else
                    return View(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(id);
            }

        }

        public async Task<IActionResult> Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return View(id);
            }
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user is not null)
                {

                    return View(new DetailsUserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RoleNames = await _userManager.GetRolesAsync(user)
                    });
                }

                else
                    return View(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(id);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");

            if (!ModelState.IsValid)
            {
                return View(id);
            }
          
            try
            {
               

                var user = await _userManager.FindByIdAsync(id.ToString());
                if(user is not null)
                {
                    return View(new DetailsUserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RoleNames = await _userManager.GetRolesAsync(user)
                    });

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UserName Is Already Registered!");
                    return View(id);

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(id);
            }
            

        }

        [HttpPost]
        public async Task<IActionResult> Edit(DetailsUserDTO model)
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");

            if (!ModelState.IsValid )
            {
               return View(model);
            }
            try
            {
              
                //if (await _userManager.FindByEmailAsync(model.Email) is not null)
                //{
                //    ModelState.AddModelError(string.Empty, "email is already found!");
                //    return View(model);
                //}



                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (user is not null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result);
                    var resultRole=await _userManager.AddToRolesAsync(user,model.RoleNames??new List<string>()) ;
                    var resultUpdated=await _userManager.UpdateAsync(user);
                    if (resultUpdated.Succeeded )
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach(var item in resultUpdated.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }
                        return View(model);
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UserName Is Already Registered!");
                    return View(model);

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

        }

    }
}
