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
            if (cat == null )
            {
                return BadRequest("Category is missing");
            }

            CreateOrUpdate(cat);

            return Ok();
        }

        [HttpGet]
        [ActionName("CheckUnique")]
        public IHttpActionResult CheckUnique(string catName,int id)
        {
            string cName = Encoding.UTF8.GetString(Convert.FromBase64String(catName));

            return Ok(!DataHandling.Categories.Any(c => String.Equals(c.Name, cName, StringComparison.InvariantCultureIgnoreCase) && c.ID != id));
        }


        private void CreateOrUpdate(Category cat)
        {
            Category actualCat = null;

            //Is it a new cat?
            if (cat.ID == -1)
            {
                actualCat = cat; //Apparently not
                actualCat.ID = DataHandling.Categories.Count;
                DataHandling.Categories.Add(actualCat);
            }
            else
            {
                //Find the actual cat
                actualCat = DataHandling.Categories.FirstOrDefault(c => c.ID == cat.ID);
            }

            actualCat.Active = cat.Active;
            actualCat.Name = cat.Name;

            //Expense/Income & ID is read-only - so no support to change it

            actualCat.Subcats = cat.Subcats;

            //Go through the subcats and ID any which need idying - for now just use their position in the array
            for(int i=0; i < actualCat.Subcats.Count; i++)
            {
                if (actualCat.Subcats[i].ID == -1)
                {
                    actualCat.Subcats[i].ID = i;
                }
            }

            //Done :)
        }
    }
}
