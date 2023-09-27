using AutoMapper;
using BLL.DTOs.HRs;
using BLL.DTOs.Roles;
using BLL.DTOs.Users;
using DAL.Models;

namespace DMSTaskMVC.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();


        }

    }
}
