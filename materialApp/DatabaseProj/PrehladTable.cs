using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseProj
{
    public class PrehladTable
    {
        private DatabaseContext context;

        public List<prehlad> mPrehladTable { get; set; }

        public string mSearchText { get; set; }
    }
}
