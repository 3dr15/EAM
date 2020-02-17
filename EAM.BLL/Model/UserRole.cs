using System.Collections.Generic;

namespace EAM.BLL.Model
{
    public class UserRole
    {
        public int UserRoleID { get; set; }
        public string Role { get; set; }
        public List<User> Users { get; set; }
    }
}
