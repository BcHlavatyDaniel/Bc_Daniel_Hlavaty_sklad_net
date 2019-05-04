using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseProj
{
    public class LogTableItem
    {
        public string id_tovar { get; set; }
        public string id_uzivatela { get; set; }
        public string typ_zmeny { get; set; }
        public string popis { get; set; }
        public string time { get; set; }
    }
}
