using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ItemsTable
    {
        private DatabaseContext context;

        public int id { get; set; }
        public string nazov { get; set; }
        public double velkost { get; set; }
        public string photo { get; set; }
        public bool stav { get; set; }
        public string prichod { get; set; }
        public string odchod { get; set; }
        public double cena { get; set; }
    }
}
