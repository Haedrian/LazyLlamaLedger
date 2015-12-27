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

        [HttpGet]
        [ActionName("Categories")]
        public IHttpActionResult Category(int ID)
        {
            return Ok(DataHandling.Categories.Where(c => c.ID == ID).Select(c => new CategoryView(c, DataHandling.SubCategories.Where(sc => sc.CategoryID == c.ID).OrderBy(sc => sc.ID).ToList())));
        }

        [HttpPost]
        [ActionName("Categories")]
        public IHttpActionResult Category([FromBody]CategoryView cat)
        {
            //Check that the model is correct
            if (cat == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            //Does it have an ID ?
            Update(cat);

            return Ok();
        }

        private void Update(CategoryView cat)
        {
            //Find the actual cat
            var actualCat = DataHandling.Categories.FirstOrDefault(c => c.ID == cat.ID);

            actualCat.Active = cat.Active;
            actualCat.Name = cat.Name;

            //Find the subcats
            var subCats = DataHandling.SubCategories.Where(sc => sc.CategoryID == cat.ID);

            
        }
    }
}
