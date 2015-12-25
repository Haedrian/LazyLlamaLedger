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
        public IEnumerable<LedgerEntry> Get()
        {
            LedgerDBContext db = new LedgerDBContext();

            return db.Entries.ToArray();
        }

        [HttpGet]
        public IHttpActionResult Get(int month)
        {
            LedgerDBContext db = new LedgerDBContext();

            return Ok( db.Entries.Where(e => e.Date.Month == month).ToArray().Select(e => new LedgerEntryView(e)).ToArray());
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] LedgerEntry le)
        {
            if (le != null && ModelState.IsValid)
            {
                LedgerDBContext db = new LedgerDBContext();

                db.Entries.Add(le);

                db.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult Categories(bool activeOnly)
        {
            LedgerDBContext db = new LedgerDBContext();

            return Ok( db.Categories.Where(c => c.Active).ToArray());
        }
    }
}
