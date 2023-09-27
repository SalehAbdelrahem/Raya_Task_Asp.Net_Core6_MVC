using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Users
{
    public class CreateUserDTO : UserDTO
    {

        [Required(ErrorMessage = "The Password Required"), DataType(DataType.Password, ErrorMessage = "Invalid Password Datatype"), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Invalid Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The ConfirmPassword  Must Match The Password"), DataType(DataType.Password)]//, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "The ConfirmPassword  Must Match The Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public ICollection<int>? RoleIds { get; set; }
    }
}
