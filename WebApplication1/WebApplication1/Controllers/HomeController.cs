using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;
            return View(context.GetAllOsobyData());
        }

        public IActionResult Privacy()
        {
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;

            return View(context.GetAllOsobyData());
        }

        [HttpPost]
        public ActionResult Add(string firstname, string secondname, int phonenumber, string adresa)
        {
            MySqlConnection mSql = new MySqlConnection("Server=127.0.0.1; port = 3306; Database=bakalarkadb;Uid=root;");
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO osoba_zak(prve_meno, druhe_meno, telefonne_cislo, adresa) VALUES(@pmeno, @dmeno, @tcislo, @adresa)";
                cmd.Parameters.AddWithValue("@pmeno", firstname);
                cmd.Parameters.AddWithValue("@dmeno", secondname);
                cmd.Parameters.AddWithValue("@tcislo", phonenumber);
                cmd.Parameters.AddWithValue("@adresa", adresa);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == System.Data.ConnectionState.Open) mSql.Close();
            }
            DatabaseContext context = HttpContext.RequestServices.GetService(typeof(DatabaseContext)) as DatabaseContext;

            return View("Index",context.GetAllOsobyData());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

/*
*/
