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
        [Route("hello")]
        public IEnumerable<string> Test()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
