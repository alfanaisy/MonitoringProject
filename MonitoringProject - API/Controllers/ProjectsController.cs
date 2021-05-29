using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MonitoringProject___API.Base;
using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories;
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
    [Authorize(Roles = "Project Manager")]
    public class ProjectsController : BaseController<Project, ProjectRepository, int>
    {
        private readonly MyContext context;
        private readonly IGenericDapper dapper;
        public ProjectsController(ProjectRepository repository, MyContext context, IGenericDapper dapper) : base(repository)
        {
            this.context = context;
            this.dapper = dapper;
        }

        [HttpPost("create-new-project")]
        public IActionResult CreateProject(Project project)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);

            var email = jwt.Claims.First(c => c.Type == "email").Value;
            var isExist = context.Users.FirstOrDefault(u => u.Email == email);

            if (isExist != null)
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ManagerId", isExist.UserID, DbType.Int32);
                dbparams.Add("ProjectName", project.ProjectName, DbType.String);
                dbparams.Add("Description", project.Description, DbType.String);
                dbparams.Add("StartDate", project.StartDate, DbType.DateTime);
                dbparams.Add("EndDate", project.EndDate, DbType.DateTime);
                dbparams.Add("Status", project.Status, DbType.String);

                var result = System.Threading.Tasks.Task.FromResult(dapper.Insert<int>("[dbo].[SP_CreateProject]", dbparams, commandType: CommandType.StoredProcedure));

                return Ok(new { Status = "Success", Message = "Project has been created" });
            }

            return BadRequest(new { Status = "Error", Message = "Create Project failed" });
        }

        [HttpGet("get-members")]
        public List<User> GetMembers()
        {
            string query = "SELECT * FROM [dbo].[TB_M_User] WHERE RoleID=2"; 
            try
            {
                List<User> Members = dapper.GetAllNoParam<User>(query, CommandType.Text);

                return Members;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
