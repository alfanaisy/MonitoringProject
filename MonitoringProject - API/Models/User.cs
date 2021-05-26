using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringProject___API.Models
{
    [Table("TB_M_User")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public Account Account { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<ModuleUser> ModuleUsers { get; set; }
        public ICollection<TaskUser> TaskUsers { get; set; }
        public ICollection<UserReport> UserReports { get; set; }
    }
}
