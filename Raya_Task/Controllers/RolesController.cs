using BLL.DTOs.Roles;
using BLL.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace Raya_Task.Controllers
{
    [Authorize(Roles ="Admin,HR_Admin,HR")]
    public class RolesController : Controller
    {
        private readonly ServiceRole _serviceRole;

        public RolesController(ServiceRole serviceRole)
        {
            _serviceRole = serviceRole;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public IActionResult GetAllRoles()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);

            var searchValue = Request.Form["search[value]"];

            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];


            IQueryable<RoleDTO> roles = _serviceRole.GetRoles()
              .Where(m => string.IsNullOrEmpty(searchValue) ? true : m.Name.Contains(searchValue));

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                roles = roles.OrderBy(string.Concat(sortColumn, " ", sortColumnDirection));



            var data = roles.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = roles.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }
        [Authorize(Roles = "HR,HR_Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleDTO role)
        {
            try
            {

                var result = await _serviceRole.CreateRole(role);
                if (result)
                {
                    return RedirectToAction("Index", "Roles");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "can not create Role");
                    return View(role);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(role);
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
                if (await _serviceRole.DeleteRole(id))
                    return RedirectToAction("Index");
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
                var roleDetails = await _serviceRole.GetRoleDetailsById(id);
                if (roleDetails is not null)
                {
                    return View(roleDetails);
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
            if (!ModelState.IsValid)
            {
                return View(id);
            }
            try
            {
               

                var role = await _serviceRole.GetRoleById(id);
                if (role is not null)
                {
                    return View(role);
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

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RoleDTO model)
        {
            if (!ModelState.IsValid || id != model.Id)
            {
                return View(model);
            }
            try
            {

                var role = await _serviceRole.GetRoleById(id);
                if (role is not null)
                {
                   
                    role.Name = model.Name;
                   
                    if (await _serviceRole.UpdateRole(role))
                    {
                        return RedirectToAction("Index");
                    }
                    else { return View(model); }
                }

                else
                    return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(id);
            }

        }

    }
}
