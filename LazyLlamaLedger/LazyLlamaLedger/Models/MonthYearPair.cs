using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public struct MonthYearPair
    {
        public int Month;
        public int Year;

        public MonthYearPair(int month, int year)
        {
            this.Month = month;
            this.Year = year;
        }

        public MonthYearPair(DateTime datetime)
        {
            this.Month = datetime.Month;
            this.Year = datetime.Year;
        }

        /// <summary>
        /// Creates a month year pair by reading the details in the ledger entry
        /// </summary>
        /// <param name="le"></param>
        public MonthYearPair(LedgerEntry le)
        {
            this.Month = le.Date.Month;
            this.Year = le.Date.Year;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(MonthYearPair))
            {
                return false; //Type mismatch
            }
            else
            {
                MonthYearPair compareObject = (MonthYearPair)obj;

                if (compareObject.Month == this.Month && compareObject.Year == this.Year)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
