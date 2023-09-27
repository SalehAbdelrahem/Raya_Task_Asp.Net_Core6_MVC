using AutoMapper;
using BLL.DTOs.HRs;
using DAL.Contacts.HRS;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BLL.Services.HRs
{
    public class ServiceHR
    {
        private readonly IHrRepository _hrRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServiceHR(IHrRepository hrRepository, 
            IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _hrRepository = hrRepository;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IEnumerable<EmployeeDTO>> GetEmployees()
        {


            try
            {
                var result = await _hrRepository.GetAllAsync();
                if (result is null)
                {
                    throw new Exception("Not Fount Any Employee");

                }
                else
                {
                    return _mapper.Map<IEnumerable<EmployeeDTO>>(result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IQueryable<EmployeeDTO> GetEmployeesAsQuerable()
        {


            try
            {
                var result =  _hrRepository.GetAllEmployeesAsQuerableAsync().Select(emp=>new EmployeeDTO
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    HireDate = emp.HireDate,
                    Salary = emp.Salary,
                    Status = emp.Status
                });
                if (result is null)
                {
                    throw new Exception("Not Fount Any Empyee");

                }
                else
                {
                    return  result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmployeeDTO> GetEmployeeById(int id)
        {


            try
            {
                var result = await _hrRepository.GetByIdAsync(id);
                if (result is null)
                {
                    throw new Exception("Not Fount Any Employee");

                }
                else
                {
                    return _mapper.Map<EmployeeDTO>(result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddEmployee(EmployeeDTO employeeDTO)
        {
            try
            { 
                var tempEmployee = _mapper.Map<Employee>(employeeDTO);

                tempEmployee.User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                var HrEmp = await _hrRepository.CreateAsync(tempEmployee);
                if (HrEmp is null)
                {
                    throw new Exception($"Can Not Creat An Employee For name = {employeeDTO.Name} ");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<bool> UpdateEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                var result = await _hrRepository.GetByIdAsync(employeeDTO.Id);
                if (result is null)
                {
                    throw new Exception("Not Found the Employee");
                }
                else
                {
                    result.Name = employeeDTO.Name;
                    result.Status = employeeDTO.Status;
                    result.Salary= employeeDTO.Salary;
                    result.HireDate = employeeDTO.HireDate;
                  return  await _hrRepository.UpdateAsync(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            try
            {
                var result = await _hrRepository.GetByIdAsync(employeeId);
                if (result is null)
                {
                    throw new Exception("Not Found the Employee");
                }
                else
                {
                  return  await _hrRepository.DeleteAsync(employeeId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
