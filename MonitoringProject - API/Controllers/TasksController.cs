using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;
using MonitoringProject___API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        //[HttpGet("get-project-id")]
        //public int GetProjectId(int taskId)
        //{
        //    var dbparams = new DynamicParameters();
        //    dbparams.Add("TaskId", taskId, DbType.Int32);

        //    int projectId = dapper.Get<int>("[dbo].[SP_GetProjectId]", dbparams, commandType: CommandType.StoredProcedure);

        //    return projectId;
        //}

        [HttpGet("get-task-by-module/{id}")]
        public List<Task> GetTaskByModule(int id)
        {
            try
            {
                string query = string.Format("SELECT T.TaskID, T.TaskName, T.Description, T.StartDate, T.EndDate, T.Status, T.Priority FROM TB_M_Task AS T WHERE T.ModuleID={0}", id);

                List<Task> tasks = dapper.GetAllNoParam<Task>(query, CommandType.Text);

                return tasks;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("get-assigned-task/{id}")]
        public List<Task> GetAssignedTask(int id)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var email = jwt.Claims.First(c => c.Type == "email").Value;
                var user = context.Users.FirstOrDefault(u => u.Email == email);

                var dbparams = new DynamicParameters();
                dbparams.Add("projectId", id, DbType.Int32);
                dbparams.Add("userId", user.UserID, DbType.Int32);

                List<Task> tasks = dapper.GetAll<Task>("SP_GetAssignedTask", dbparams, CommandType.StoredProcedure);

                return tasks;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
