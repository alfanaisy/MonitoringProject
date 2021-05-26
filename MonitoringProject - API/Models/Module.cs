using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringProject___API.Models
{
    [Table("TB_M_Module")]
    public class Module
    {
        [Key]
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<ModuleUser> ModuleUsers { get; set; }
    }
}
