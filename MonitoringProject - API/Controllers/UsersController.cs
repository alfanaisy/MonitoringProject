using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;
using MonitoringProject___API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController<User, UserRepository, int>
    {
        private readonly UserRepository repository;
        private readonly IGenericDapper dapper;
        public UsersController(UserRepository repository, IGenericDapper dapper) : base(repository)
        {
            this.repository = repository;
            this.dapper = dapper;
        }

        [HttpGet("get-members")]
        [Authorize(Roles = "Project Manager")]
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

        [HttpGet("get-project-members/{id}")]
        [Authorize(Roles = "Project Manager")]
        public List<User> GetProjectMembers(int id)
        {
            
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ProjectId", id, DbType.Int32);
                List<User> Members = dapper.GetAll<User>("[dbo].[SP_GetProjectMembers]", dbparams, CommandType.StoredProcedure);

                return Members;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
