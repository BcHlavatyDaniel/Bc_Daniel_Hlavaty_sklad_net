using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using System.Net;
using MySql.Data.MySqlClient;

namespace WebApplication3.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            return View("Table",context.GetPrehladTable());
        }

        private void ftp_download(string name)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("test", "test");
            client.DownloadFile("ftp://dokelu.kst.fri.uniza.sk/" + name, "wwwroot/resources/" + name);
        }

        [HttpPost]
        public ActionResult LoadImage(string datat)
        {
            int id = Convert.ToInt32(datat);
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            string name = context.GetImageName(id);
            string path = System.IO.Path.GetFullPath("wwwroot/resources/" + name);

            if (!System.IO.File.Exists(path))
            {
                ftp_download(name);
            }

            JsonResult jresult = new JsonResult("resources/" + name);
            return jresult;
        }
    }
}