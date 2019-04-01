using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace materialApp
{
    class EditItemStruct
    {
        public string id { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public string price { get; set; }
        public string photo { get; set; }
        public DateTime created_at { get; set; }
        public DateTime returned_at { get; set; }
        public DateTime sold_at { get; set; }
        public DateTime paid_at { get; set; }
        public string keyy { get; set; }
        public string keyn { get; set; }
    }
}
