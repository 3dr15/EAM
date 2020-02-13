using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAM.DAL.Context
{
    public static class PrepDB
    {
        private readonly EamContext _context;

        public static PrepDB(EamContext context)
        {
            _context = context;
            SeedDB();
        }

        private void SeedDB()
        {
            if(this.context.UserRoles == null || this.context.UserRoles.ToList().Count() == 0)
            {
                this.context.UserRoles.AddRange(
                    new Entity.UserRole { Role = "Administrator"},
                    new Entity.UserRole { Role = "SystemAdministrator"},
                    new Entity.UserRole { Role = "Management"},
                    new Entity.UserRole { Role = "Employee"}
                );
            }
        }
    }
}
