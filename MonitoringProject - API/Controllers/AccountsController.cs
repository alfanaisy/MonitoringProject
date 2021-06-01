using Dapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Linq;
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
                    return Ok(token);
                    //return token;
                }

                //return "Unauthorized";
                return Unauthorized();
            }
            catch (Exception)
            {
                //return "BadRequest";
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword(Forgot forgot)
        {
            var jwt = new JwtService(config);
            var emailService = new EmailService(config, context);
            var isValid = context.Users.SingleOrDefault(u => u.Email == forgot.Email);
            if(isValid != null)
            {
                var token = jwt.GenerateSecurityToken(forgot.Email);
                var url = "https://localhost:44343/authentication/ResetPassword?token=" + token;
                string subject = "Reset Password";
                emailService.SendEmail(config.GetSection("EmailSettings").GetSection("Mail").Value, subject, forgot.Email, url);
                return Ok(new { token, Status = "Success", Message = "Email has been sent to your address with further instruction." });
            }
            return NotFound(new { Status = "Error", Message = "This email does not exist in our database." });
        }

        [HttpPut]
        [Route("reset-password")]
        public IActionResult ResetPassword(Reset reset)
        {
            try
            {
                string token = reset.Token;
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var email = jwt.Claims.First(c => c.Type == "email").Value;
                var isExist = context.Accounts.FirstOrDefault(u => u.User.Email == email);

                isExist.Password = BCrypt.Net.BCrypt.HashPassword(reset.Password);

                var result = repository.Put(isExist);
                if (result > 0)
                {
                    return Ok(new { Status = "Success", Message = "Password has been reset" });
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        [HttpPut("change-password")]
        [Authorize]
        public ActionResult ChangePassword(Change change)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);

            var email = jwt.Claims.First(c => c.Type == "email").Value;
            var isExist = context.Accounts.FirstOrDefault(u => u.User.Email == email);

            if (BCrypt.Net.BCrypt.Verify(change.OldPassword, isExist.Password))
            {
                isExist.Password = BCrypt.Net.BCrypt.HashPassword(change.NewPassword);
                var result = repository.Put(isExist);
                if (result > 0)
                {
                    return Ok(new { Status = "Success", Message = "Password has been changed" });
                }
            }

            return BadRequest();
        }

        [HttpGet("test-pm")]
        [Authorize(Roles = "Project Manager")]
        public IActionResult PMTest()
        {
            return Ok(new { Message = "Project Manager Only" });
        }
    }
}
