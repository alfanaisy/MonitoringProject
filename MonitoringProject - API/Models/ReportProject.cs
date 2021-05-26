using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Models
{
    [Table("TB_T_ReportProject")]
    public class ReportProject
    {
        public int ReportID { get; set; }
        public Report Report { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
