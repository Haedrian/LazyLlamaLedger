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
    public class CatController:ApiController
    {
        [HttpGet]
        [ActionName("Categories")]
        public IHttpActionResult Categories()
        {
            return Ok(DataHandling.Categories.OrderBy(c => c.IsExpense).ThenBy(c => c.Name).ToList());
        }

        [HttpGet]
        [ActionName("Categories")]
        public IHttpActionResult Category(int ID)
        {
            return Ok(DataHandling.Categories.Where(c => c.ID == ID).FirstOrDefault());
        }

        [HttpPost]
        [ActionName("Categories")]
        public IHttpActionResult Category([FromBody]Category cat)
        {
            //Check that the model is correct
            if (cat == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            //Does it have an ID ?
          //  Update(cat);

            return Ok();
        }

        //private void Update(CategoryView cat)
        //{
        //    //Find the actual cat
        //    var actualCat = DataHandling.Categories.FirstOrDefault(c => c.ID == cat.ID);

        //    actualCat.Active = cat.Active;
        //    actualCat.Name = cat.Name;

        //    //Find the subcats
        //    var subCats = DataHandling.SubCategories.Where(sc => sc.CategoryID == cat.ID);

            
        //}
    }
}
