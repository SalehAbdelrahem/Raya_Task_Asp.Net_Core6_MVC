using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Users
{
 
    public class UserDTO
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string LastName { get; set; }
        [Required, MinLength(3), MaxLength(100)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Email Required"), DataType(DataType.EmailAddress), RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
