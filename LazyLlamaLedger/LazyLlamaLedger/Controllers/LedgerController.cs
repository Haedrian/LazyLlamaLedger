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
        public IEnumerable<LedgerEntry> Get()
        {
            return DataHandling.LedgerEntries;
        }

        [HttpGet]
        [ActionName("LedgerEntry")]
        public IHttpActionResult Get(int month, int year)
        {
            return Ok(DataHandling.LedgerEntries.Where(e => e.Date.Month == month && e.Date.Year == year).ToArray().OrderBy(e => e.Date).Select(e => new LedgerEntryView(e)).ToArray());
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

            var monthValues = DataHandling.LedgerEntries.Where(e => e.Date.Month == month && e.Date.Year == year && e.IsExpense).ToArray();

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
            var monthValues = DataHandling.LedgerEntries.Where(e => e.Date.Month == month && e.Date.Year == year && e.Category == catID).ToArray();

            var res = monthValues.GroupBy(m => m.SubCategory).Select(cg => new { Category = DataHandling.Categories.First(c => c.ID == catID).Subcats.FirstOrDefault(sc => sc.ID == cg.FirstOrDefault().SubCategory).Name, Sum = cg.Sum(cg1 => cg1.Money).ToString("0.00") });

            return Ok(res);
        }

        [HttpGet]
        [ActionName("LedgerIncAggregate")]
        public IHttpActionResult GetIncomeAggregate(int month, int year)
        {
            //First get all the values of that month

            var monthValues = DataHandling.LedgerEntries.Where(e => e.Date.Month == month && e.Date.Year == year && !e.IsExpense).ToArray();

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
                //Incremement subcat amount
                DataHandling.Categories.FirstOrDefault(cat => le.Category == cat.ID).Subcats.FirstOrDefault(sc => sc.ID == le.ID).Uses++;

                return Ok();
            }
            else
            {
                return BadRequest();
            }
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
