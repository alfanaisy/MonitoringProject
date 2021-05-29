using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Base;
using MonitoringProject___API.Context;
using MonitoringProject___API.Models;
using MonitoringProject___API.Repositories.Data;
using MonitoringProject___API.Repositories.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MonitoringProject___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : BaseController<Report, ReportRepository, int>
    {
        private readonly MyContext context;
        private readonly IGenericDapper dapper;
        public ReportsController(ReportRepository repository, MyContext context, IGenericDapper dapper) : base(repository)
        {
            this.context = context;
            this.dapper = dapper;
        }

        [HttpPost("create-report")]
        public IActionResult CreateReport(Report report, int projectId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var jwtReader = new JwtSecurityTokenHandler();
            var jwt = jwtReader.ReadJwtToken(token);

            var email = jwt.Claims.First(c => c.Type == "email").Value;
            var isExist = context.Users.FirstOrDefault(u => u.Email == email);
            if (isExist != null)
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("Title", report.Title, DbType.String);
                dbparams.Add("Content", report.Content, DbType.String);
                dbparams.Add("ReportDate", report.ReportDate, DbType.DateTime);
                dbparams.Add("TaskId", report.TaskID, DbType.Int32);
                dbparams.Add("ProjectId", projectId, DbType.Int32);
                dbparams.Add("UserId", isExist.UserID, DbType.Int32);

                var result = System.Threading.Tasks.Task.FromResult(dapper.Insert<int>("[dbo].[SP_SubmitReport]", dbparams, commandType: CommandType.StoredProcedure));

                return Ok(new { Status = "Success", Message = "Report has been submitted" });
            }
            return BadRequest();
        }
    }
}
