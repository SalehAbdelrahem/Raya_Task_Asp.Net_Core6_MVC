using BLL.DTOs.Users;
using DAL.Models;

namespace BLL.Extensions.Users
{
    public static class RegistrationModelExtensions
    {
        public static User ToModel(this RegistrationModel registrationModel)
        {
            return new User
            {
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                UserName = registrationModel.UserName,
                Email = registrationModel.Email
            };
        }
    }

}
