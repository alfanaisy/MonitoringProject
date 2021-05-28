using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Project Manager")]
    public class ModulesController : BaseController<Module, ModuleRepository, int>
    {
        public ModulesController(ModuleRepository repository) : base(repository)
        {
        }
    }
}
