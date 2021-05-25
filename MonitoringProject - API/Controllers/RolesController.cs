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
    public class RolesController : BaseController<Role, RoleRepository, int>
    {
        public RolesController(RoleRepository repository) : base(repository)
        {
        }
    }
}
