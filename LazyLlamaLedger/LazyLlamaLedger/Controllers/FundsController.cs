using LazyLlamaLedger.Models;
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
    public class FundsController
        :ApiController
    {
        [HttpGet]
        public IHttpActionResult Funds(string fundName = null)
        {
            if (String.IsNullOrEmpty(fundName))
            {
                return Ok(DataHandling.Funds.ToArray().OrderBy(ob => ob.IsActive).ThenBy(ob => ob.Name));
            }
            else
            {
                return Ok(DataHandling.Funds.FirstOrDefault(f => f.Name.Equals(fundName, StringComparison.InvariantCultureIgnoreCase)));
            }
        }

        [HttpPost]
        public IHttpActionResult NewFund(Fund fund)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (fund == null)
            {
                return BadRequest("Fund is missing");
            }

            string error = "";

            if (!fund.CheckIfValid(out error))
            {
                return BadRequest(error);
            }

            //Check whether the name is unique
            if (DataHandling.Funds.Any(f => String.Equals(f.Name,fund.Name,StringComparison.InvariantCultureIgnoreCase)))
            {
                return BadRequest("There is a fund with that name already");
            }

            if (DataHandling.Funds.Any(f => String.Equals(f.Colour, fund.Colour, StringComparison.InvariantCultureIgnoreCase)))
            {
                return BadRequest("There is a fund with that colour already");
            }

            //Add it to the funds
            DataHandling.Funds.Add(fund);

            DataHandling.FlushFunds(); //To be sure

            return Ok();
        }

    }
}
