using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAM.DAL.Context
{
    public class PrepDB
    {
        private readonly EamContext _context;
        
        public PrepDB(EamContext context)
        {
            _context = context;
            SeedDB();
        }

        private void SeedDB()
        {
            if(this._context.UserRoles == null || this._context.UserRoles.ToList().Count() == 0)
            {
                this._context.UserRoles.AddRange(
                    new Entity.UserRole { Role = "Administrator"},
                    new Entity.UserRole { Role = "SystemAdministrator"},
                    new Entity.UserRole { Role = "Management"},
                    new Entity.UserRole { Role = "Employee"}
                );
            }
        }
    }
}
