using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using System.Net;
using MySql.Data.MySqlClient;
using DatabaseProj;

namespace WebApplication3.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            PrehladTable currTable = context.GetPrehladTable("");
            foreach (var item in currTable.mPrehladTable)
            {
                LoadImage2(item.photo);
            }
            return View("Table", currTable);
        }

        private void LoadImage2(string name)
        {
            string path = System.IO.Path.GetFullPath("wwwroot/resources/" + name);

            if (!System.IO.File.Exists(path))
            {
                if (!System.IO.Directory.Exists("wwwroot/resources"))
                {
                    System.IO.Directory.CreateDirectory("wwwroot/resources");
                }
                if (!(name == null) && !(name == ""))
                {
                    ftp_download(name);
                }
            }
        }

        private void ftp_download(string name)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("test", "test");
            client.DownloadFile("ftp://dokelu.kst.fri.uniza.sk/" + name, "wwwroot/resources/" + name);
        }

        [HttpPost]
        public ActionResult Index(PrehladTable prehladTable)
        {
            string iwonder = prehladTable.mSearchText;
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            PrehladTable currTable = context.GetPrehladTable(prehladTable.mSearchText);

            foreach (var item in currTable.mPrehladTable)
            {
                LoadImage2(item.photo);
            }
            return View("Table", currTable);
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
                if (!System.IO.Directory.Exists("wwwroot/resources"))
                {
                    System.IO.Directory.CreateDirectory("wwwroot/resources");
                }
                if (!(name == null) && !(name == ""))
                    ftp_download(name);
                else return new JsonResult("No image");
            }

            JsonResult jresult = new JsonResult("resources/" + name);
            return jresult;
        }
    }
}