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
    public class UsersController : BaseController<User, UserRepository, int>
    {
        private readonly UserRepository repository;
        public UsersController(UserRepository repository) : base(repository)
        {
            this.repository = repository;
        }
    }
}
