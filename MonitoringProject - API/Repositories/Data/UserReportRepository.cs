using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Repositories.Data
{
    public class UserReportRepository : GeneralRepository<UserReport, int>
    {
        public UserReportRepository(MyContext context) : base(context)
        {
        }
    }
}
