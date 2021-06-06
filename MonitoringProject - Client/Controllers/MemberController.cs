using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.Models;
using MonitoringProject___API.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MonitoringProject___Client.Controllers
{
    [Authorize(Roles = "Project Member")]
    public class MemberController : Controller
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
                ViewData["controller"] = "Member";
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
                ViewData["controller"] = "Member";
                return View();
            }
            return RedirectToAction("Index", "Authentication");
        }

        [HttpGet]
        public string GoTask(int id)
        {
            HttpContext.Session.SetInt32("projectId", id);
            return Url.Action("Task", "Member");
        }

        public IActionResult Task()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;

                ViewData["name"] = name;
                ViewData["controller"] = "Member";
                ViewData["projectId"] = projectId;
                return View();
            }
            return RedirectToAction("Index", "Authentication");
        }

        public IActionResult Profile()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;

                ViewData["name"] = name;
                ViewData["controller"] = "Member";
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

        public List<Task> GetTasks()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/tasks/get-assigned-task/{0}", projectId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var tasks = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<Task>>(tasks);
                    return data;
                }
            }
            return null;
        }

        public HttpStatusCode FinishTask(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/tasks/{0}", id)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var tasks = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Task>(tasks);

                    data.Status = "Finished";

                    StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                    var updateStatus = client.PutAsync("https://localhost:44380/api/tasks", content).Result;

                    if (updateStatus.IsSuccessStatusCode)
                    {
                        return HttpStatusCode.OK;
                    }
                    else
                    {
                        return HttpStatusCode.BadRequest;
                    }
                }
            }
            return HttpStatusCode.Unauthorized;
        }
        public HttpStatusCode AddReport(Report report)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(report), Encoding.UTF8, "application/json");
                var result = client.PostAsync(string.Format("https://localhost:44380/api/Reports/create-report/{0}", projectId), stringContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        public IActionResult ChangePasswordAPI(Change change)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(change), Encoding.UTF8, "application/json");
            var result = client.PutAsync("https://localhost:44380/api/accounts/change-password", stringContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(new { result });
            }
            else
            {
                return BadRequest(new { result });
            }
        }
    }
}
