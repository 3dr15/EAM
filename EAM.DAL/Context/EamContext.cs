using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAM.DAL.Context
{
    public class EamContext : DbContext
    {
        public EamContext(DbContextOptions<EamContext> options) : base(options)
        {

        }

        public DbSet<Entity.UserRole> UserRoles {get; set;}
        public DbSet<Entity.User> Users { get; set; }
        public DbSet<Entity.Card> Cards { get; set; }
        public DbSet<Entity.Attendance> Attendances { get; set; }
    }
}
