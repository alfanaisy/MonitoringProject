using Microsoft.AspNetCore.Mvc;
using MonitoringProject___API.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringProject___Client.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
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
            return View();
        }

        public HttpStatusCode ForgotPasswordAPI(Forgot forgot)
        {
            var client = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(forgot), Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://localhost:44380/api/accounts/forgot-password", stringContent).Result;
            return result.StatusCode;
        }
    }
}
