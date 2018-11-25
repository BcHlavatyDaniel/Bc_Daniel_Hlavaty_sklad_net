using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Parser
    {
        public Parser()
        {

        }

        public string ParseItem(DatItem item)
        {

            string ret = "Name :      " + item.GetFirstName() + "\n";
            ret += "SurName:    " + item.GetSecondName() + "\n";
            switch(item.WhoAmI())
            {
                case '1':  
                    ret += "Weight :    ";
                    break;
                case '2':
                    ret += "Count :     ";
                    break;
                case '3':
                    ret += "SmthnElse : "; ;
                    break;
                default:
                    break;
            }
            ret += Convert.ToString(item.GetStat()) + "\n";

            ret += "Datum ulozenia tovaru na sklade : " + item.GetTimeStart().ToString() + "\n" ;
            ret += "Datum odchodu zo skladu : " + item.GetTimeEnd().ToString() + "\n";
            ret += "------------------------------------------------------";

            return ret;
        }
    }
}
