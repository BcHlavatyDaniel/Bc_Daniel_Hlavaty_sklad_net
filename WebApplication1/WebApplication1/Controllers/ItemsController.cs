using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            return View("Items", context.GetAllItemsData());
        }

        [HttpPost]
        public ActionResult LoadImage(string datat)
        {
            int id = Convert.ToInt32(datat);

            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            string path = context.GetImagePath(id);

            JsonResult jresult = new JsonResult(path);
            return jresult;
        }
    }
}