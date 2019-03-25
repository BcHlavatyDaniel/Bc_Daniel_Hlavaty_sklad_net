using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class TovarTable
    {
        private DatabaseContext context;

        public int id { get; set; }
        public string nazov { get; set; }
        public double velkost { get; set; }
        public double cena { get; set; }
        public string foto { get; set; }
    }
}
