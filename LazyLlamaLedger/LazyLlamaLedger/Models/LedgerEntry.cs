using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LazyLlamaLedger.Models
{
    public class LedgerEntry
    {
        public int ID { get; set; }
        public string Item { get; set; }
        public bool IsExpense { get; set; }
        public DateTime Date { get; set; }
    }

    public class LedgerDBContext: DbContext
    {
        public DbSet<LedgerEntry> Entries { get; set; }
    }
}
