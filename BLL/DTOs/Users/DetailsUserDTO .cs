using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Users
{
    public class DetailsUserDTO : UserDTO
    {

        [DisplayName("Role Names")]
        public ICollection<string>? RoleNames { get; set; }
    }
}
