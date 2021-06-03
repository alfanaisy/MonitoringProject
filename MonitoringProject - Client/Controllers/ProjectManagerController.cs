using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Http;

namespace MonitoringProject___Client.Controllers
{
    [Authorize(Roles = "Project Manager")]
    public class ProjectManagerController : Controller
    {
        public IActionResult Index()
        {
            var jwtCookie = HttpContext.Request.Cookies["jwt-cookie"];
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;
                
                ViewData["name"] = name;
                ViewData["controller"] = "ProjectManager";
                ViewData["jwtCookie"] = jwtCookie;
                return View();
            }
            return RedirectToAction("Index", "Authentication");
        }

        public IActionResult Project()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;

                ViewData["name"] = name;
                ViewData["controller"] = "ProjectManager";
                return View();
            }
            return RedirectToAction("Index", "Authentication");
        }

        public List<Project> GetProjects()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync("https://localhost:44380/api/projects/get-project-by-user").Result;
                if (result.IsSuccessStatusCode)
                {
                    var projects = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<Project>>(projects);
                    return data;
                }
            }
            return null;
        }

        public HttpStatusCode AddProject(Project project)
        {
            var token = HttpContext.Session.GetString("JWToken");
            
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");
                var result = client.PostAsync("https://localhost:44380/api/projects/create-new-project", stringContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }
    }
}
