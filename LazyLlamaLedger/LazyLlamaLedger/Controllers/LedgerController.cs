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
    [EnableCors("*","*","*")]
    public class LedgerController:ApiController
    {
        [HttpGet]
        [ActionName("LedgerEntry")]
        public IEnumerable<LedgerEntry> Get()
        {
            return DataHandling.LedgerEntries;
        }

        [HttpGet]
        [ActionName("LedgerEntry")]
        public IHttpActionResult Get(int month)
        {
            return Ok( DataHandling.LedgerEntries.Where(e => e.Date.Month == month).ToArray().Select(e => new LedgerEntryView(e)).ToArray());
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
        public IHttpActionResult Category(bool activeOnly)
        {            
            if (activeOnly)
            {
                return Ok(DataHandling.Categories.Where(c => c.Active));
            }
            else
            {
                return Ok(DataHandling.Categories);
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
