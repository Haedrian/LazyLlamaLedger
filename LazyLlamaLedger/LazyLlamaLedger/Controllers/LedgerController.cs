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

        [HttpGet]
        [ActionName("LedgerAggregate")]
        public IHttpActionResult GetAggregate(int month, int year)
        {
            //First get all the values of that month

            var monthValues = DataHandling.LedgerEntries.Where(e => e.Date.Month == month && e.Date.Year == year).ToArray();

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
                return Ok(DataHandling.Categories.Where(c => c.Active && c.IsExpense == expense).ToArray());
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
