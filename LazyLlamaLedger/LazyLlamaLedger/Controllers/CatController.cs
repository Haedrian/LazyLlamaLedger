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
    public class CatController:ApiController
    {
        [HttpGet]
        [ActionName("Categories")]
        public IHttpActionResult Categories()
        {
            return Ok(DataHandling.Categories.Select(c => new CategoryView(c, DataHandling.SubCategories.Where(sc => sc.CategoryID == c.ID).ToList())));
        }
    }
}
