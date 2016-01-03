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
    public class ReportController
        : ApiController
    {

        public IHttpActionResult GetTable(DateTime startDate,DateTime endDate)
        {
            return Ok(); //TODO
        }

    }
}
