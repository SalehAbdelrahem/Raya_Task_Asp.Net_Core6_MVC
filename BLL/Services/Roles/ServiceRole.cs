using AutoMapper;
using BLL.DTOs.HRs;
using BLL.DTOs.Roles;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace BLL.Services.Roles
{
    public class ServiceRole
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public ServiceRole(
            IMapper mapper,
            RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {

            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public  IQueryable<RoleDTO> GetRoles()
        {
            try
            {
                IQueryable<RoleDTO> result = _roleManager.Roles.Select(r=>new RoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                });
                if (result is null)
                {
                    throw new Exception("Not Fount Any Role");

                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateRole(RoleDTO roleDTO)
        {
            try
            {
                var tempRole = _mapper.Map<Role>(roleDTO);
                var role = await _roleManager.CreateAsync(tempRole);
                
                if (!role.Succeeded)
                {
                    var errors= new StringBuilder();
                    foreach (var error in role.Errors)
                        errors.Append($"{error.Description} , ");
                    throw new Exception($"Can Not Creat An Role For name = {roleDTO.Name} - messages =  {errors.ToString()}" );
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<RoleDTO> GetRoleById(int id)
        {


            try
            {
                var result = await _roleManager.FindByIdAsync(id.ToString());
                if (result is null)
                {
                    throw new Exception("Not Fount Any Role");

                }
                else
                {
                    return _mapper.Map<RoleDTO>(result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoleDetailsWithUsersNameDTO> GetRoleDetailsById(int id)
        {


            try
            {
                var result = await _roleManager.FindByIdAsync(id.ToString());
                var user =await _userManager.GetUsersInRoleAsync(result.Name);
                if (result is null )
                {
                    throw new Exception("Not Fount Any Role");

                }
                else
                {
                    return new RoleDetailsWithUsersNameDTO()
                    {
                        Id=result.Id,
                        Name=result.Name,
                        UsersName= user.Select(x => x.UserName).ToList()
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            try
            {
                var result = await _roleManager.FindByIdAsync(roleId.ToString());
                if (result is null)
                {
                    throw new Exception("Not Found the Role");
                }
                else
                {
                    var role= await _roleManager.DeleteAsync(result);
                    if (!role.Succeeded)
                    {
                        var errors = new StringBuilder();
                        foreach (var error in role.Errors)
                            errors.Append($"{error.Description} , ");
                        throw new Exception($"Can Not Delete the Role For RoleID = {roleId} - messages =  {errors.ToString()}");

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateRole(RoleDTO roleDTO)
        {
            try
            {
                var result = await _roleManager.FindByIdAsync(roleDTO.Id.ToString());
                if (result is null)
                {
                    throw new Exception("Not Found the Role");
                }
                else
                {
                    result.Name = roleDTO.Name;
                    var IsUpdated= await _roleManager.UpdateAsync(result);
                    if (IsUpdated.Succeeded)
                    {
                        return true;
                    }
                    else
                    {
                        var errors=new StringBuilder();
                        foreach (var item in IsUpdated.Errors)
                        {
                            errors.Append($" {item.Description} ,  ");   
                        }
                        throw new Exception(errors.ToString());
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
