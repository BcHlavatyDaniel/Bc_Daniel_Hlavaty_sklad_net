﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using MySql.Data.MySqlClient;
using System.Net;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string pass)
        {
            JsonResult jresult;

            if (login == "root" && pass == "root")
            {
                jresult = new JsonResult("YES");
            }
            else
            {
                jresult = new JsonResult("NO");
            }

            return jresult;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}