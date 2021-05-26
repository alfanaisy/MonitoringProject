using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Models
{
    [Table("TB_T_TaskUser")]
    public class TaskUser
    {
        public int TaskID { get; set; }
        public Task Task { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
