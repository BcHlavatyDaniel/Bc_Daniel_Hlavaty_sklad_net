using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class UserTable
    {
        private DatabaseContext context;

        private int id_year { get; set; }
        private int id_numbers { get; set; }
        private string f_name { get; set; }
        private string s_name { get; set; }
        private string address { get; set; }
        private int phone { get; set; }
    }
}
