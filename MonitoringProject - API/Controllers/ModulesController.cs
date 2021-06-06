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
    [Authorize(Roles = "Project Manager")]
    public class ModulesController : BaseController<Module, ModuleRepository, int>
    {
        private readonly MyContext context;
        private readonly IGenericDapper dapper;
        public ModulesController(ModuleRepository repository, MyContext context, IGenericDapper dapper) : base(repository)
        {
            this.context = context;
            this.dapper = dapper;
        }

        [HttpGet("get-module-by-project/{id}")]
        public List<Module> GetModuleByProject(int id)
        {
            try
            {
                string query = string.Format("SELECT M.ModuleID, M.ModuleName, M.Description, M.StartDate, M.EndDate, M.Status FROM TB_M_Module AS M WHERE M.ProjectID={0}", id);

                List<Module> modules = dapper.GetAllNoParam<Module>(query, CommandType.Text);

                return modules;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
