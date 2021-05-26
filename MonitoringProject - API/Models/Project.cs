using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringProject___API.Models
{
    [Table("TB_M_Project")]
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<ReportProject> ReportProjects { get; set; }
    }
}
