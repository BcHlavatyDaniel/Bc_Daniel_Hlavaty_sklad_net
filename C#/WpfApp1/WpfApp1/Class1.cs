using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class DatItem
    {
        string mFirstName;
        string mSecondName;
        double mStat;
        DateTime mTimeStart;
        DateTime mTimeEnd;
        char mType;
        //bool mCheckedOut;


        public DatItem()
        {
            mFirstName = "";
            mSecondName = "";
            mStat = 0;
            mTimeStart = DateTime.Now.Date;
            mTimeEnd = DateTime.Now.Date;
            mType = '0';
        }

        public void SetFirstName(string name)
        {
            mFirstName = name;
        }

        public void SetSecondName(string name)
        {
            mSecondName = name;
        }

        public void SetStat(double stat)
        {
            mStat = stat;
        }

        public void SetTimeStart(DateTime time)
        {
            mTimeStart = time;
        }

        public void SetTimeEnd(DateTime time)
        {
            mTimeEnd = time;
        }

        public string GetFirstName()
        {
            return mFirstName;
        }

        public void SetWhoIAm(char type)
        {
            mType = type;
        }

        public string GetSecondName()
        {
            return mSecondName;
        }

        public double GetStat()
        {
            return mStat;
        }

        public DateTime GetTimeStart()
        {
            return mTimeStart;
        }

        public DateTime GetTimeEnd()
        {
            return mTimeEnd;
        }

        public char WhoAmI()
        {
            return mType;
        }

    }
}
