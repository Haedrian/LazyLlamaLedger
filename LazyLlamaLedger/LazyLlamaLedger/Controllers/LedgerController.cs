using LazyLlamaLedger.Models;
using LazyLlamaLedger.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LazyLlamaLedger.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LedgerController : ApiController
    {
        [HttpGet]
        [ActionName("LedgerEntry")]
        public IHttpActionResult Get(int month, int year)
        {
            return Ok(DataHandling.GetLedgerEntries(year,month).ToArray().OrderBy(e => e.Date).Select(e => new LedgerEntryView(e)).ToArray());
        }

        /// <summary>
        /// Returns aggregate data for EXPENSES
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("LedgerExpAggregate")]
        public IHttpActionResult GetExpenseAggregate(int month, int year)
        {
            //First get all the values of that month

            var monthValues = DataHandling.GetLedgerEntries(year,month).Where(e => e.IsExpense).ToArray();

            //Then group them by category and return the cat and the sum
            var res = monthValues.GroupBy(m => m.Category).Select(cg => new { Category = DataHandling.Categories.Where(c => c.ID == cg.First().Category).Select(c => c.Name).FirstOrDefault(), Sum = cg.Sum(cg1 => cg1.Money).ToString("0.00") });

            return Ok(res);
        }

        [HttpGet]
        [ActionName("LedgerSubcategoryAggregate")]
        public IHttpActionResult GetSubCatAggregate(int month, int year, string catName)
        {
            //Decode the base64-ness of it all
            string catN = Encoding.UTF8.GetString( Convert.FromBase64String(catName));

            //Determine the category by name
            var catID = DataHandling.Categories.FirstOrDefault(c => String.Equals(c.Name, catN, StringComparison.InvariantCultureIgnoreCase)).ID;

            //First get all the values of that month
            var monthValues = DataHandling.GetLedgerEntries(year,month).Where(e => e.Category == catID).ToArray();

            var res = monthValues.GroupBy(m => m.SubCategory).Select(cg => new { Category = DataHandling.Categories.First(c => c.ID == catID).Subcats.FirstOrDefault(sc => sc.ID == cg.FirstOrDefault().SubCategory).Name, Sum = cg.Sum(cg1 => cg1.Money).ToString("0.00") });

            return Ok(res);
        }

        [HttpGet]
        [ActionName("LedgerIncAggregate")]
        public IHttpActionResult GetIncomeAggregate(int month, int year)
        {
            //First get all the values of that month

            var monthValues = DataHandling.GetLedgerEntries(year, month).Where(e => !e.IsExpense).ToArray();

            //Then group them by category and return the cat and the sum
            var res = monthValues.GroupBy(m => m.Category).Select(cg => new { Category = DataHandling.Categories.Where(c => c.ID == cg.First().Category).Select(c => c.Name).FirstOrDefault(), Sum = cg.Sum(cg1 => cg1.Money).ToString("0.00") });

            return Ok(res);
        }

        [HttpPost]
        [ActionName("LedgerEntry")]
        public IHttpActionResult Post([FromBody] LedgerEntry le)
        {
            if (le != null && ModelState.IsValid)
            {
                DataHandling.AddLedgerEntry(le);

                //Increment the use amount
                DataHandling.Categories.FirstOrDefault(cat => le.Category == cat.ID).Uses++;

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ActionName("CollectiveLedgerEntry")]
        public IHttpActionResult Post([FromBody] CollectiveLedgerEntry le)
        {
            if (le == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (le.DateFrom >= le.DateTo)
            {
                return BadRequest("Date from isn't larger than Date to");
            }

            //We're going to create a number of ledger entries - one for each month.
            //Let's determine how many days there are between date from and the end of the month
            //Then we'll work month by month, adding days as appropriate
            //Then we'll determine each ratio, and divide the total money by that amount
            List<Tuple<DateTime, int>> ratios = new List<Tuple<DateTime, int>>();

            int daysInFirstMonth = DateTime.DaysInMonth(le.DateFrom.Year, le.DateFrom.Month) - le.DateFrom.Day + 1;

            ratios.Add(new Tuple<DateTime, int>(new DateTime(le.DateFrom.Year,le.DateFrom.Month,1),daysInFirstMonth));

            //Now, go month month until we reach the enddate
            DateTime dateCursor = new DateTime(le.DateFrom.Ticks);

            dateCursor = dateCursor.AddMonths(1);

            while (dateCursor.Month < le.DateTo.Month)
            {
                ratios.Add(new Tuple<DateTime, int>(new DateTime(dateCursor.Year, dateCursor.Month, 1), DateTime.DaysInMonth(dateCursor.Year, dateCursor.Month)));

                dateCursor = dateCursor.AddMonths(1);
            }

            //This is the enddate month, so let's do similar to what we did with firstmonth
            int daysInLastMonth = le.DateTo.Day;

            ratios.Add(new Tuple<DateTime, int>(new DateTime(le.DateTo.Year, le.DateTo.Month, 1), daysInLastMonth));

            //Now we create the entries

            int daysTotal = ratios.Sum(r => r.Item2);

            foreach(var ratio in ratios)
            {
                LedgerEntry entry = new LedgerEntry()
                {
                    Category = le.Category,
                    Date = ratio.Item1,
                    IsCollective = true,
                    IsExpense = le.IsExpense,
                    Item = le.Item,
                    Money = Math.Round(((le.Money / (decimal)daysTotal) * ratio.Item2), 2),
                    SubCategory = le.SubCategory
                };

                DataHandling.AddLedgerEntry(entry);
            }

            return Ok();
        }

        [HttpGet]
        [ActionName("Category")]
        public IHttpActionResult Category(bool activeOnly, bool expense)
        {
            if (activeOnly)
            {
                return Ok(DataHandling.Categories.Where(c => c.Active && c.IsExpense == expense).OrderByDescending(c => c.Uses).ThenBy(c => c.Name).ToArray());
            }
            else
            {
                return Ok(DataHandling.Categories.Where(c => c.IsExpense == expense).ToArray());
            }
        }

        [HttpPost]
        [ActionName("Category")]
        public IHttpActionResult Category([FromBody]Category cat)
        {
            if (cat != null && ModelState.IsValid)
            {
                DataHandling.AddCategory(cat);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
