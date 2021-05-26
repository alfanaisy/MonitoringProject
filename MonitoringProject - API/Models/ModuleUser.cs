using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Models
{
    [Table("TB_T_ModuleUser")]
    public class ModuleUser
    {
        public int ModuleID { get; set; }
        public Module Module { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
