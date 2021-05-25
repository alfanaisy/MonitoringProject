using Microsoft.EntityFrameworkCore;
using MonitoringProject___API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Context
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(roleId => roleId.RoleID);
        }
    }
}
