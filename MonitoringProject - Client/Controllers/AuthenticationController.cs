using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
