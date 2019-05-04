using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseProj
{
    public class User
    {
		public string Name
		{
			get
			{
				return $"{FName} {SName}";
			}
		}
		public string IdName
		{
			get
			{
				return $"{IdYear}-{IdNumber}";
			}
			set
			{
				string[] x = value.Split('-');
				IdYear = int.Parse(x[0]);
				IdNumber = int.Parse(x[1]);
			}
		}
        public string Id { get; set; }
        public int IdYear { get; set; }
        public int IdNumber { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
    }
}
