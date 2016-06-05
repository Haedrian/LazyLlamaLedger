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

        [HttpPut]
        public IHttpActionResult UpdateFund(string fundName,[FromBody]UpdateFund fund)
        {
            if (String.IsNullOrEmpty(fundName))
            {
                return BadRequest("Fund Name is missing");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string error = "";
            if (!fund.CheckIfValid(out error))
            {
                return BadRequest(error);
            }

            //Get the fund from the database
            var oldFund = DataHandling.Funds.FirstOrDefault(f => f.Name.Equals(fundName, StringComparison.InvariantCultureIgnoreCase));

            if (oldFund == null)
            {
                return BadRequest("Could not find fund with name " + fundName);
            }

            //Update old fund
            oldFund.IsActive = fund.IsActive;
            oldFund.MaximumAmount = fund.MaximumAmount;
            oldFund.MinimumAmount = fund.MinimumAmount;
            oldFund.MinimumIfNegative = fund.MinimumIfNegative;
            oldFund.Percentage = fund.Percentage;

            //Done
            DataHandling.FlushFunds();

            return Ok();
        }

        [HttpPatch]
        public IHttpActionResult AdjustTotals(string fundName,decimal newTotal)
        {
            //Get the fund from the database
            var oldFund = DataHandling.Funds.FirstOrDefault(f => f.Name.Equals(fundName, StringComparison.InvariantCultureIgnoreCase));

            if (oldFund == null)
            {
                return BadRequest("Could not find fund with name " + fundName);
            }

            //Update old fund
            oldFund.Total = Math.Round(newTotal,2);

            DataHandling.FlushFunds();

            return Ok();
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
