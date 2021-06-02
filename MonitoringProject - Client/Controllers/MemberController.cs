﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
