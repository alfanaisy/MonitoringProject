using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonitoringProject___API.Base;
using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;
using MonitoringProject___API.Repositories.Interfaces;
using MonitoringProject___API.Services;
using MonitoringProject___API.ViewModels;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, int>
    {
        private readonly AccountRepository repository;
        private readonly IGenericDapper dapper;
        private readonly MyContext context;
        private readonly IConfiguration config;
        
        public AccountsController(AccountRepository repository, IGenericDapper dapper, MyContext context, IConfiguration config) : base(repository)
        {
            this.repository = repository;
            this.dapper = dapper;
            this.context = context;
            this.config = config;
        }

        [HttpPost("register-member")]
        public IActionResult RegisterMember(Register register)
        {
            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);
                
                var dbparams = new DynamicParameters();
                dbparams.Add("Name", register.Name, DbType.String);
                dbparams.Add("Email", register.Email, DbType.String);
                dbparams.Add("Password", hashPassword, DbType.String);
                dbparams.Add("RoleId", 2, DbType.Int32);

                var result = System.Threading.Tasks.Task.FromResult(dapper.Insert<int>("[dbo].[SP_Register]", dbparams, commandType: CommandType.StoredProcedure));

                return Ok(new { Status = "Success", Message = "User has been registered successfully." });
            }
            catch (Exception)
            {
                return BadRequest(new { Status = "Failed", Message = "User Registration Failed." });
            }
        }

        [HttpPost("register-manager")]
        public IActionResult RegisterManager(Register register)
        {
            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);

                var dbparams = new DynamicParameters();
                dbparams.Add("Name", register.Name, DbType.String);
                dbparams.Add("Email", register.Email, DbType.String);
                dbparams.Add("Password", hashPassword, DbType.String);
                dbparams.Add("RoleId", 1, DbType.Int32);

                var result = System.Threading.Tasks.Task.FromResult(dapper.Insert<int>("[dbo].[SP_Register]", dbparams, commandType: CommandType.StoredProcedure));

                return Ok(new { Status = "Success", Message = "User has been registered successfully." });
            }
            catch (Exception)
            {
                return BadRequest(new { Status = "Failed", Message = "User Registration Failed." });
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser(Login login)
        {
            var jwt = new JwtService(config);
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("Email", login.Email, DbType.String);
                dynamic result = dapper.Get<dynamic>(
                    "[dbo].[SP_Login]",
                    dbparams,
                    CommandType.StoredProcedure
                    );

                if (BCrypt.Net.BCrypt.Verify(login.Password, result.Password))
                {
                    var token = jwt.GenerateSecurityToken(result.Name, result.Email, result.Role);
                    return Ok(new { token, name = result.Name, role = result.Role });
                }

                return Unauthorized();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}
