using System.ComponentModel;

namespace BLL.DTOs.Roles
{
    public class RoleDTO
    {
        public int Id { get; set; }
        [DisplayName("Role Name")]
        public string Name { get; set; }
    }
}
