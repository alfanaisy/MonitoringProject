using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Models
{
    [Table("TB_M_Role")]
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<User> Users { get; set; }
    }
}
