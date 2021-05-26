using MonitoringProject___API.Context;
using MonitoringProject___API.Models;

namespace MonitoringProject___API.Repositories.Data
{
    public class ReportRepository : GeneralRepository<Report, int>
    {
        public ReportRepository(MyContext context) : base(context)
        {
        }
    }
}
