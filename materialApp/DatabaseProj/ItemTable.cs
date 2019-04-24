using DatabaseProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseProj
{
    public class ItemTable
    {
        private DatabaseContext context;

        public int id { get; set; }
        public string name { get; set; }
        public int user_year { get; set; }
        public int user_numbers { get; set; }
        public string size { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public string photo { get; set; }
        public int stav { get; set; }
        public bool archived { get; set; }
    }
}
