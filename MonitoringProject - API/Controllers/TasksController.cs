using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController<Task, TaskRepository, int>
    {
        public TasksController(TaskRepository repository) : base(repository)
        {
        }
    }
}
