using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringProject___API.Models
{
    [Table("TB_M_Account")]
    public class Account
    {
        [ForeignKey("User")]
        public int AccountID { get; set; }
        public string Password { get; set; }
        public User User { get; set; }
    }
}
