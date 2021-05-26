using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Models
{
    [Table("TB_T_UserReport")]
    public class UserReport
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int ReportID { get; set; }
        public Report Report { get; set; }
    }
}
