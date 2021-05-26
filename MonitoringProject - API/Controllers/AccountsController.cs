using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MonitoringProject___API.Base;
using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, int>
    {
        private readonly AccountRepository repository;
        public AccountsController(AccountRepository repository) : base(repository)
        {
            this.repository = repository;
        }
    }
}
