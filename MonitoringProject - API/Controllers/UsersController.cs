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

        [HttpGet("get-members/{id}")]
        [Authorize(Roles = "Project Manager")]
        public List<User> GetMembers(int id)
        {
            
            try
            {
                var dbparam = new DynamicParameters();
                dbparam.Add("ProjectId", id, DbType.Int32);
                List<User> Members = dapper.GetAll<User>("[dbo].[SP_GetMemberCoba]", dbparam, CommandType.StoredProcedure);

                return Members;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("get-project-members")]
        [Authorize(Roles = "Project Manager")]
        public List<User> GetProjectMembers(int id, int taskId)
        {
            
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("ProjectId", id, DbType.Int32);
                dbparams.Add("TaskId", taskId, DbType.Int32);
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
