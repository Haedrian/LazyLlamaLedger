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
    [EnableCors("*", "*", "*")]
    public class ReportController
        : ApiController
    {

        [HttpGet]
        [ActionName("ExpenseTable")]
        public IHttpActionResult GetExpenseTable(DateTime startDate,DateTime endDate)
        {
            //So, first we get every category we have
            var allCats = DataHandling.Categories;

            //Create an empty report row for each
            ReportTable rt = new ReportTable();
            rt.Rows = new List<ReportRow>();
            rt.Headers = new List<string>();

            foreach(var cat in allCats)
            {
                ReportRow rr = new ReportRow();
                rr.CategoryName = cat.Name;
                rr.CatID = cat.ID; //for easier matching
                rr.Values = new List<string>();

                rt.Rows.Add(rr);
            }

            LedgerController lc = new LedgerController();

            //Now start getting monthly aggregate data
            DateTime dateTimeCursor = startDate;

            while (dateTimeCursor <= endDate)
            {
                //Get the data

                var monthValues = DataHandling.LedgerEntries.Where(e => e.Date.Month == dateTimeCursor.Month && e.Date.Year == dateTimeCursor.Year && e.IsExpense).ToArray();

                //Then group them by category and return the cat and the sum
                var res = monthValues.GroupBy(m => m.Category).Select(cg => new { Category = DataHandling.Categories.Where(c => c.ID == cg.First().Category).Select(c => c.ID).FirstOrDefault(), Sum = cg.Sum(cg1 => cg1.Money).ToString("0.00") });

                //Create a new header for each
                rt.Headers.Add(dateTimeCursor.Year + "/" + dateTimeCursor.Month);

                //Now, go through the rows, and add a new value, with the value from the res if possible, or 0 if not
                foreach(var row in rt.Rows)
                {
                    var datum = res.FirstOrDefault(w => w.Category == row.CatID);

                    if (datum == null)
                    {
                        row.Values.Add("0");
                    }
                    else
                    {
                        row.Values.Add(datum.Sum);
                    }
                }

                dateTimeCursor = dateTimeCursor.AddMonths(1);
            }

            //Clean up, go through all cats which have no data in them, and remove them
            for(int i=0; i < rt.Rows.Count; i++)
            {
                if (!rt.Rows[i].Values.Any(v => v != "0"))
                {
                    //delete it
                    rt.Rows.RemoveAt(i);
                    i--;
                }
            }


            return Ok(rt); //TODO
        }

        [HttpGet]
        [ActionName("IncomeTable")]
        public IHttpActionResult GetIncomeTable(DateTime startDate, DateTime endDate)
        {
            //So, first we get every category we have
            var allCats = DataHandling.Categories;

            //Create an empty report row for each
            ReportTable rt = new ReportTable();
            rt.Rows = new List<ReportRow>();
            rt.Headers = new List<string>();

            foreach (var cat in allCats)
            {
                ReportRow rr = new ReportRow();
                rr.CategoryName = cat.Name;
                rr.CatID = cat.ID; //for easier matching
                rr.Values = new List<string>();

                rt.Rows.Add(rr);
            }

            LedgerController lc = new LedgerController();

            //Now start getting monthly aggregate data
            DateTime dateTimeCursor = startDate;

            while (dateTimeCursor <= endDate)
            {
                //Get the data

                var monthValues = DataHandling.LedgerEntries.Where(e => e.Date.Month == dateTimeCursor.Month && e.Date.Year == dateTimeCursor.Year && !e.IsExpense).ToArray();

                //Then group them by category and return the cat and the sum
                var res = monthValues.GroupBy(m => m.Category).Select(cg => new { Category = DataHandling.Categories.Where(c => c.ID == cg.First().Category).Select(c => c.ID).FirstOrDefault(), Sum = cg.Sum(cg1 => cg1.Money).ToString("0.00") });

                //Create a new header for each
                rt.Headers.Add(dateTimeCursor.Year + "/" + dateTimeCursor.Month);

                //Now, go through the rows, and add a new value, with the value from the res if possible, or 0 if not
                foreach (var row in rt.Rows)
                {
                    var datum = res.FirstOrDefault(w => w.Category == row.CatID);

                    if (datum == null)
                    {
                        row.Values.Add("0");
                    }
                    else
                    {
                        row.Values.Add(datum.Sum);
                    }
                }

                dateTimeCursor = dateTimeCursor.AddMonths(1);
            }

            //Clean up, go through all cats which have no data in them, and remove them
            for (int i = 0; i < rt.Rows.Count; i++)
            {
                if (!rt.Rows[i].Values.Any(v => v != "0"))
                {
                    //delete it
                    rt.Rows.RemoveAt(i);
                    i--;
                }
            }


            return Ok(rt); //TODO
        }

        [HttpGet]
        [ActionName("Totals")]
        public IHttpActionResult GetTotals(DateTime startDate,DateTime endDate)
        {
            List<string> dates = new List<string>();
            List<decimal> incomes = new List<decimal>();
            List<decimal> expenses = new List<decimal>();
            List<decimal> total = new List<decimal>();

            DateTime dateTimeCursor = startDate;

            while (dateTimeCursor <= endDate)
            {
                dates.Add(startDate.ToString("yy/MM"));

                incomes.Add(DataHandling.LedgerEntries.Where(le => le.Date.Month == dateTimeCursor.Month && le.Date.Year == dateTimeCursor.Year && !le.IsExpense).Sum(e => e.Money));
                expenses.Add(DataHandling.LedgerEntries.Where(le => le.Date.Month == dateTimeCursor.Month && le.Date.Year == dateTimeCursor.Year && le.IsExpense).Sum(e => e.Money));
                total.Add(incomes[incomes.Count-1] - expenses[expenses.Count -1]);

                dateTimeCursor = dateTimeCursor.AddMonths(1);
            }

            return Ok(
                new
                {
                    s1 = expenses,
                    s2 = incomes,
                    s3 = total,
                    ticks = dates
                });
        }
    }
}
