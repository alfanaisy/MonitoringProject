using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.ViewModels
{
    public class ReportVM
    {
        public int ReportID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReportDate { get; set; }
        public string TaskName { get; set; }
        public string Name { get; set; }
    }
}
