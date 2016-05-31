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
        public IHttpActionResult GetStateOfMonth(int year, int month)
        {
            var monthData =DataHandling.ClosedMonths.FirstOrDefault(cm => cm.Month == month && cm.Year == year);

            if (monthData == null)
            {
                return Ok(monthData); //There we go
            }
            else
            {
                //Let's expand the data we sent back so we include pretty colours

                return Ok(monthData.FundDistribution.Select(fd => new { Name =  fd.Key, Amount = fd.Value, Colour = DataHandling.Funds.Where( f => f.Name.Equals(fd.Key,StringComparison.InvariantCultureIgnoreCase)).Select(f => f.Colour).FirstOrDefault() } ).ToArray());
            }
        }

        [HttpPatch]
        public IHttpActionResult CloseMonth(int year, int month)
        {
            //We must distribute according to the funds
            //How much money we make?
            decimal total = DataHandling.GetLedgerEntries(year, month).Where(le => !le.IsExpense).Sum(le => le.Money) - DataHandling.GetLedgerEntries(year, month).Where(le => le.IsExpense).Sum(le => le.Money);

            //Go through the funds and see how much we need to give
            List<KeyValuePair<string, decimal>> fundDistribution = new List<KeyValuePair<string, decimal>>();

            foreach(var fund in DataHandling.Funds.Where(f => f.IsActive))
            {
                decimal amount = 0;

                if (total < 0)
                {
                    if (fund.MinimumIfNegative && fund.MinimumAmount.HasValue)
                    {
                        amount = fund.MinimumAmount.Value;
                    }
                    else
                    {
                        amount = 0;
                    }
                }
                else
                {
                    //Calculate the percentage
                    if (fund.Percentage.HasValue)
                    {
                        amount = fund.Percentage.Value * total / 100;

                        //are we above max?
                        amount = Math.Max(amount, fund.MaximumAmount ?? 0);
                    } 
                    else
                    {
                        //Just minimum
                        amount = fund.MinimumAmount.Value;
                    }
                }

                fundDistribution.Add(new KeyValuePair<string, decimal>(fund.Name, amount));
                fund.Total += amount;

            }

            DataHandling.ClosedMonths.Add(new Models.MonthDetails() { Month = month, Year = year });
            DataHandling.FlushClosedMonths();
            DataHandling.FlushFunds();

            return Ok();
        }

    }
}
