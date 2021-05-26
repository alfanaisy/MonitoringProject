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
        public DbSet<Module> Modules { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ModuleUser> ModuleUsers { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<UserReport> UserReports { get; set; }
        public DbSet<ReportProject> ReportProjects { get; set; }

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
                .HasKey(mu => new { mu.ModuleID, mu.UserID });
            //user - task
            modelBuilder.Entity<TaskUser>()
                .HasKey(tu => new { tu.TaskID, tu.UserID });
            //user - report
            modelBuilder.Entity<UserReport>()
                .HasKey(ur => new { ur.ReportID, ur.UserID });
            //project - report
            modelBuilder.Entity<ReportProject>()
                .HasKey(rp => new { rp.ReportID, rp.ProjectID });
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
