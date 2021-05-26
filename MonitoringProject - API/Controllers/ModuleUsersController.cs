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
    public class ModuleUsersController : BaseController<ModuleUser, ModuleUserRepository, int>
    {
        public ModuleUsersController(ModuleUserRepository repository) : base(repository)
        {
        }
    }
}
