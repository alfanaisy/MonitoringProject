using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportProjectsController : BaseController<ReportProject, ReportProjectRepository, int>
    {
        public ReportProjectsController(ReportProjectRepository repository) : base(repository)
        {
        }
    }
}
