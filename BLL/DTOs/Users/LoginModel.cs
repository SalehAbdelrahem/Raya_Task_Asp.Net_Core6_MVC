using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Users
{
    public class LoginModel
    {
        [Required, MinLength(3), MaxLength(200)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Password Required"), DataType(DataType.Password, ErrorMessage = "Invalid Password Datatype"), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Invalid Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public LoginModel() { }
        public LoginModel(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }

}
