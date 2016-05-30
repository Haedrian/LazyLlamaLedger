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
    public class MonthController
        :ApiController
    {

        [HttpGet]
        public IHttpActionResult GetStateOfMonth(int month, int year)
        {
            return Ok(DataHandling.ClosedMonths.Any(cm => cm.Month == month && cm.Year == year));
        }

        [HttpPatch]
        public IHttpActionResult CloseMonth(int month, int year)
        {
            DataHandling.ClosedMonths.Add(new Models.ClosedMonth() { Month = month, Year = year });
            DataHandling.FlushClosedMonths();

            return Ok();
        }

    }
}
