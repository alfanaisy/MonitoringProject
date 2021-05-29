using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;
using MonitoringProject___API.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Project Manager")]
    public class TasksController : BaseController<Task, TaskRepository, int>
    {
        private readonly MyContext context;
        private readonly IGenericDapper dapper;
        public TasksController(TaskRepository repository, MyContext context, IGenericDapper dapper) : base(repository)
        {
            this.context = context;
            this.dapper = dapper;
        }

        [HttpGet("get-project-members")]
        public List<User> GetProjectMembers(int projectId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("ProjectId", projectId, DbType.Int32);

            List<User> Members = dapper.GetAll<User>("[dbo].[SP_GetProjectMembers]", dbparams, commandType: CommandType.StoredProcedure);

            return Members;
        }

        //or use Session instead
        [HttpGet("get-project-id")]
        public int GetProjectId(int taskId)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("TaskId", taskId, DbType.Int32);

            int projectId = dapper.Get<int>("[dbo].[SP_GetProjectId]", dbparams, commandType: CommandType.StoredProcedure);

            return projectId;
        }
    }
}
