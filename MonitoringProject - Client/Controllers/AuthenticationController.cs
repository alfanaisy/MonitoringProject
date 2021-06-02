using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringProject___Client.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("JWToken");
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterProjectManager()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            if (Request.Query.ContainsKey("token"))
            {
                var token = Request.Query["token"].ToString();
                ViewData["token"] = token;
                return View();
            }
            return NotFound();
        }

        public IActionResult ForgotPasswordAPI(Forgot forgot)
        {
            var client = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(forgot), Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://localhost:44380/api/accounts/forgot-password", stringContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(new { result });
            }
            else
            {
                return BadRequest(new { result });
            }
        }

        public IActionResult ResetPasswordAPI(Reset reset)
        {
            var client = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(reset), Encoding.UTF8, "application/json");
            var result = client.PutAsync("https://localhost:44380/api/accounts/reset-password", stringContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok(new { result });
            }
            else
            {
                return BadRequest(new { result });
            }
        }

        public object LoginAPI(Login login)
        {
            var client = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://localhost:44380/api/accounts/login", stringContent).Result;
            var token = result.Content.ReadAsStringAsync().Result;

            HttpContext.Session.SetString("JWToken", token);

            if (result.IsSuccessStatusCode)
            {
                var jwtReader = new JwtSecurityTokenHandler();
                var jwt = jwtReader.ReadJwtToken(token);

                var role = jwt.Claims.First(c => c.Type == "role").Value;

                Response.Cookies.Append("jwt-cookie", token);

                if (role == "Project Manager")
                {
                    return Url.Action("Index", "ProjectManager");
                }
                else if (role == "Project Member")
                {
                    return Url.Action("Index", "Member");
                }
                else
                {
                    return Url.Action("Error", "Home");
                }
            }
            else
            {
                return Url.Action("Error", "Home");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Index", "Authentication");
        }

        public string RegisterProjectManagerAPI(Register register)
        {
            var client = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://localhost:44380/api/accounts/register-manager", stringContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return Url.Action("Index", "Authentication");
            }
            else
            {
                return Url.Action("Error", "Home");
            }
        }

        public string RegisterMemberAPI(Register register)
        {
            var client = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://localhost:44380/api/accounts/register-member", stringContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return Url.Action("Index", "Authentication");
            }
            else
            {
                return Url.Action("Error", "Home");
            }
        }
    }
}
