using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http;

namespace MonitoringProject___Client.Controllers
{
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
                var role = jwt.Claims.First(c => c.Type == "role").Value;
                if(role == "Project Manager")
                {
                    ViewData["name"] = name;
                    return View();
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
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
                var role = jwt.Claims.First(c => c.Type == "role").Value;
                ViewData["role"] = "ProjectManager";
                if (role == "Project Manager")
                {
                    ViewData["name"] = name;
                    return View();
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            return RedirectToAction("Index", "Authentication");
        }
    }
}
