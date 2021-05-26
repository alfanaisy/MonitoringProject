using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringProject___API.Models
{
    [Table("TB_M_Report")]
    public class Report
    {
        [Key]
        public int ReportID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReportDate { get; set; }
        public int TaskID { get; set; }
        public Task Task { get; set; }
        public ICollection<UserReport> UserReports { get; set; }
        public ICollection<ReportProject> ReportProjects { get; set; }
    }
}
