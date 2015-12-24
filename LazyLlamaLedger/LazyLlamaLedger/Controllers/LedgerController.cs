using LazyLlamaLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LazyLlamaLedger.Controllers
{
    public class LedgerController:ApiController
    {
        [HttpGet]
        public IEnumerable<LedgerEntry> Get()
        {
            LedgerDBContext db = new LedgerDBContext();

            return db.Entries.ToArray();
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
    }
}
