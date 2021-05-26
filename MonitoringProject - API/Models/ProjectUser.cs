using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Models
{
    [Table("TB_T_ProjectUsers")]
    public class ProjectUser
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
