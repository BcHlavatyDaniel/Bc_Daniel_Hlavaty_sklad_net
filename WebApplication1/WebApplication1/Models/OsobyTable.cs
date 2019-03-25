using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OsobyTable
    {
        private DatabaseContext context;

        public int id { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public int phoneNumber { get; set; }
        public string  address { get; set; }
    }
}
