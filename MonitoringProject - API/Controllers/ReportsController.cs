using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : BaseController<Report, ReportRepository, int>
    {
        public ReportsController(ReportRepository repository) : base(repository)
        {
        }
    }
}
