using LazyLlamaLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Views
{
    public class LedgerEntryView
    {
        public int ID { get; set; }

        public string Item { get; set; }
        public bool IsExpense { get; set; }
        public string Date { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public decimal Money { get; set; }

        public LedgerEntryView()
        { }

        public LedgerEntryView(LedgerEntry le)
        {
            ID = le.ID;
            Item = le.Item;
            IsExpense = le.IsExpense;
            Date = le.Date.ToString("yyyy-MM-dd");
            Category = DataHandling.Categories.Where(c => c.ID == le.Category).Select(c => c.Name).FirstOrDefault();
            SubCategory = DataHandling.SubCategories.Where(c => c.ID == le.SubCategory).Select(c => c.Name).FirstOrDefault(); ;
            Money = le.Money;
        }
    }
}
