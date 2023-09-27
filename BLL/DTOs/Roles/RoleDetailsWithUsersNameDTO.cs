using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.Roles
{
    public class RoleDetailsWithUsersNameDTO
    {
        public int Id { get; set; }
        [DisplayName("Role Name")]
        public string Name { get; set; }
        [DisplayName("List Users Names")]

        public ICollection<string>? UsersName { get; set; }

    }
}
