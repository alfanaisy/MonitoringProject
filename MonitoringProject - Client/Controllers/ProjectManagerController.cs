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
using Task = MonitoringProject___API.Models.Task;
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

        //PROJECTS START
        public IActionResult Project()
        {
            HttpContext.Session.Remove("projectId");
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
        public HttpStatusCode DeleteProject(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.DeleteAsync(string.Format("https://localhost:44380/api/Projects/{0}", id)).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        //PROJECTS END

        //MODULE START
        public IActionResult Module()
        {
            HttpContext.Session.Remove("moduleId");
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null && projectId != 0)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;

                ViewData["name"] = name;
                ViewData["controller"] = "ProjectManager";
                ViewData["projectId"] = projectId;
                return View();
            }
            return RedirectToAction("Index","Authentication");
        }

        [HttpGet]
        public List<Module> GetModules()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/modules/get-module-by-project/{0}", projectId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var modules = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<Module>>(modules);
                    return data;
                }
            }
            return null;
        }

        public HttpStatusCode AddModule(Module module)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");

            module.ProjectID = projectId.Value;

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(module), Encoding.UTF8, "application/json");
                var result = client.PostAsync("https://localhost:44380/api/modules", stringContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        public HttpStatusCode DeleteModule(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.DeleteAsync(string.Format("https://localhost:44380/api/Modules/{0}", id)).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        //MODULE END

        //TASK START
        public IActionResult Task()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            var moduleId = HttpContext.Session.GetInt32("moduleId");
            if (token != null)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;

                ViewData["name"] = name;
                ViewData["controller"] = "ProjectManager";
                ViewData["projectId"] = projectId;
                ViewData["moduleId"] = moduleId;
                return View();
            }
            return RedirectToAction("Index", "Authentication");
        }

        public List<Task> GetTasks()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var moduleId = HttpContext.Session.GetInt32("moduleId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/tasks/get-task-by-module/{0}", moduleId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var tasks = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<Task>>(tasks);
                    return data;
                }
            }
            return null;
        }

        public HttpStatusCode AddTask(Task task)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var moduleId = HttpContext.Session.GetInt32("moduleId");

            task.ModuleID = moduleId.Value;

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
                var result = client.PostAsync("https://localhost:44380/api/tasks", stringContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        public HttpStatusCode DeleteTask(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.DeleteAsync(string.Format("https://localhost:44380/api/tasks/{0}", id)).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        //TASK END

        //OTHER HANDLER START

        [HttpGet]
        public string GoModule(int id)
        {
            HttpContext.Session.SetInt32("projectId", id);
            return Url.Action("Module", "ProjectManager");
        }

        [HttpGet]
        public string GoTask(int id)
        {
            HttpContext.Session.SetInt32("moduleId", id);
            return Url.Action("Task", "ProjectManager");
        }

        public List<User> GetMembers()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync("https://localhost:44380/api/users/get-members").Result;
                if (result.IsSuccessStatusCode)
                {
                    var members = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<User>>(members);
                    return data;
                }
            }
            return null;
        }

        public HttpStatusCode AddMember(ProjectUser projectUser)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");

            projectUser.ProjectID = projectId.Value;

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(projectUser), Encoding.UTF8, "application/json");
                var result = client.PostAsync("https://localhost:44380/api/projectusers", stringContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        public List<User> GetProjectMembers()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/users/get-project-members/{0}", projectId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var members = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<User>>(members);
                    return data;
                }
            }
            return null;
        }

        public HttpStatusCode AssignMember(TaskUser taskUser)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(taskUser), Encoding.UTF8, "application/json");
                var result = client.PostAsync("https://localhost:44380/api/taskusers", stringContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    return HttpStatusCode.OK;
                }
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Unauthorized;
        }

        //OTHER HANDLER END

        public Project GetProjectById()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/Projects/{0}", projectId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var projects = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Project>(projects);
                    return data;
                }
            }
            return null;
        }

        public Module GetModuleById()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var moduleId = HttpContext.Session.GetInt32("moduleId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/Modules/{0}", moduleId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var modules = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Module>(modules);
                    return data;
                }
            }
            return null;
        }



        //CHART START

        public List<object> GetModuleChart()
        {
            var modules = GetModules();
            List<object> data = new List<object>();
            for (int i = 0; i < modules.Count; i++)
            {
                string color = "";
                if(modules[i].Status == "Finished")
                {
                    color = "#078a00";
                }
                else if(modules[i].Status == "In-Progress")
                {
                    color = "#008FFB";
                }
                else
                {
                    color = "#8d0000";
                }
                data.Add(new
                {
                    x = modules[i].ModuleName,
                    y = new dynamic[]{
                            MilliTimeStamp(modules[i].StartDate),
                            MilliTimeStamp(modules[i].EndDate)
                        },
                    fillColor = color
                });
            }
            return data;
        }

        public List<object> GetTaskChart()
        {
            var tasks = GetTasks();
            List<object> data = new List<object>();
            for (int i = 0; i < tasks.Count; i++)
            {
                string color = "";
                if (tasks[i].Status == "Finished")
                {
                    color = "#078a00";
                }
                else if (tasks[i].Status == "In-Progress")
                {
                    color = "#008FFB";
                }
                else
                {
                    color = "#8d0000";
                }
                data.Add(new
                {
                    x = tasks[i].TaskName,
                    y = new dynamic[]{
                            //modules[i].StartDate.ToString("yyyy-MM-dd"),
                            //modules[i].EndDate.ToString("yyyy-MM-dd")
                            MilliTimeStamp(tasks[i].StartDate),
                            MilliTimeStamp(tasks[i].EndDate)
                        },
                    fillColor = color
                });
            }
            return data;
        }

        //CHART END

        public double MilliTimeStamp(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = TheDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return ts.TotalMilliseconds;
        }

        [HttpGet]
        public string GoReport(int id)
        {
            HttpContext.Session.SetInt32("projectId", id);
            return Url.Action("Report", "ProjectManager");
        }

        public IActionResult Report()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null && projectId != 0)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var name = jwt.Claims.First(c => c.Type == "unique_name").Value;

                ViewData["name"] = name;
                ViewData["controller"] = "ProjectManager";
                ViewData["projectId"] = projectId;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }

        [HttpGet]
        public List<dynamic> GetReports()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var projectId = HttpContext.Session.GetInt32("projectId");
            if (token != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(string.Format("https://localhost:44380/api/reports/get-report-by-project/{0}", projectId)).Result;
                if (result.IsSuccessStatusCode)
                {
                    var reports = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(reports);
                    return data;
                }
            }
            return null;
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
                ViewData["controller"] = "ProjectManager";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Authentication");
            }
        }
    }
}
