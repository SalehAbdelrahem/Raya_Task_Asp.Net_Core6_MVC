using BLL.DTOs.HRs;
using BLL.Enums.HRs;
using BLL.Services.HRs;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;
namespace RayaTaskMVC.Controllers.HR
{

    public class HRsController : Controller
    {
        private readonly ServiceHR _serviceHR;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HRsController(ServiceHR serviceHR,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _serviceHR = serviceHR;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetAllEmployees()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);

            var searchValue = Request.Form["search[value]"];

            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            //IQueryable<EmployeeDTO> employees = _context.Employees
            //    .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.Salary.Equals(searchValue)))
            //    .Select(emp=>new EmployeeDTO
            //    {
            //        Id = emp.Id,
            //        Name = emp.Name,
            //        Salary = emp.Salary,
            //        HireDate = DateTime.Now,
            //        Status=emp.Status

            //    });
            IQueryable<EmployeeDTO> employees = _serviceHR.GetEmployeesAsQuerable()
             .Where(m => string.IsNullOrEmpty(searchValue) ? true : (m.Name.Contains(searchValue) || m.Salary.Equals(searchValue)));

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                employees = employees.OrderBy(string.Concat(sortColumn, " ", sortColumnDirection));



            var data = employees.Skip(skip).Take(pageSize).ToList();

            var recordsTotal = employees.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Ok(jsonData);
        }
        [Authorize(Roles = "HR,HR_Admin")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user is not null && await _userManager.IsInRoleAsync(user, "HR_Admin"))
            {
                ViewBag.Status = new SelectList(typeof(HrStatus).GetEnumNames());
            }
            else
            {
                ViewBag.Status = new SelectList(new List<string>()
                {
                    (typeof(HrStatus).GetEnumNames()).FirstOrDefault()
                });
            }
            //ViewBag.Status = new SelectList(typeof(HrStatus).GetEnumNames());
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "HR,HR_Admin")]


        public async Task<IActionResult> Create(EmployeeDTO employee)
        {
            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if (user is not null && await _userManager.IsInRoleAsync(user, "HR_Admin"))
                {
                    ViewBag.Status = new SelectList(typeof(HrStatus).GetEnumNames());
                }
                else
                {
                    ViewBag.Status = new SelectList(new List<string>()
                {
                    (typeof(HrStatus).GetEnumNames()).FirstOrDefault()
                });
                }
                // ViewBag.Status = new SelectList(typeof(HrStatus).GetEnumNames());

                var result = await _serviceHR.AddEmployee(employee);
                if (result)
                {
                    return RedirectToAction("Index", "HRs");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "can not create Employee");
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employee);
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
                if (await _serviceHR.DeleteEmployee(id))
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
                var emp = await _serviceHR.GetEmployeeById(id);
                if (emp is not null)
                {
                    return View(emp);
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
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if (user is not null && await _userManager.IsInRoleAsync(user, "HR_Admin"))
                {
                    ViewBag.Status = new SelectList(typeof(HrStatus).GetEnumNames());
                }
                else
                {
                    ViewBag.Status = new SelectList(
                        new List<string>() { (typeof(HrStatus).GetEnumNames()).FirstOrDefault() }
                        );
                }

                var emp = await _serviceHR.GetEmployeeById(id);
                if (emp is not null)
                {
                    return View(emp);
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
        public async Task<IActionResult> Edit(int id, EmployeeDTO model)
        {
            if (!ModelState.IsValid || id != model.Id)
            {
                return View(model);
            }
            try
            {

                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

                if (user is not null && await _userManager.IsInRoleAsync(user, "HR_Admin"))
                {
                    ViewBag.Status = new SelectList(typeof(HrStatus).GetEnumNames());
                }
                else
                {
                    ViewBag.Status = new SelectList(
                        new List<string>() { (typeof(HrStatus).GetEnumNames()).FirstOrDefault() }
                        );
                }

                var emp = await _serviceHR.GetEmployeeById(id);
                if (emp is not null)
                {
                    emp.Status = model.Status;
                    emp.Name = model.Name;
                    emp.Salary = model.Salary;
                    emp.HireDate = model.HireDate;
                    if (await _serviceHR.UpdateEmployee(emp))
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
