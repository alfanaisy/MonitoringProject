using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
//using System.Web.Http;

namespace MonitoringProject___Client.Controllers
{
    [Authorize(Roles = "Project Manager")]
    public class ProjectManagerController : Controller
    {
        public IActionResult Index()
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
                var result = client.GetAsync("https://localhost:44380/api/projects").Result;
                if (result.IsSuccessStatusCode)
                {
                    var projects = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<Project>>(projects);
                    return data;
                }
            }
            return null;
        }
    }
}
