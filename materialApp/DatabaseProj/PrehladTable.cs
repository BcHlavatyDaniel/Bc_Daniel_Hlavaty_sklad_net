using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseProj
{
    public class PrehladTable
    {
        private DatabaseContext context;

        public string id_item { get; set; }
        public string id_user { get; set; }
        public string s_name { get; set; }
        public string f_name { get; set; }
        public string item_name { get; set; }
        public string price { get; set; }
        public string size { get; set; }
        public string state { get; set; }
        public string photo { get; set; }
    }
}
