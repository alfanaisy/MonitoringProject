using Microsoft.EntityFrameworkCore;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Context
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //user - role
            modelBuilder.Entity<User>()
                .HasOne(r => r.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(r => r.RoleID);
            //user - account
            modelBuilder.Entity<User>()
                .HasOne(a => a.Account)
                .WithOne(u => u.User);
            //user - project
            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectID, pu.UserID });
            //user - module
            modelBuilder.Entity<ModuleUser>()
                .HasKey(pu => new { pu.ModuleID, pu.UserID });
            //user - task
            modelBuilder.Entity<TaskUser>()
                .HasKey(pu => new { pu.TaskID, pu.UserID });
            //user - report
            modelBuilder.Entity<UserReport>()
                .HasKey(pu => new { pu.ReportID, pu.UserID });
            //project - report
            modelBuilder.Entity<ReportProject>()
                .HasKey(pu => new { pu.ReportID, pu.ProjectID });
            //project - module
            modelBuilder.Entity<Module>()
                .HasOne(m => m.Project)
                .WithMany(p => p.Modules)
                .HasForeignKey(m => m.ProjectID);
            //module - task
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Module)
                .WithMany(m => m.Tasks)
                .HasForeignKey(t => t.ModuleID);

        }
    }
}
