using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class ReportTable
    {
        public List<ReportRow> row;
    }

    public class ReportRow
    {
        public string categoryName;
        public List<decimal> values;
    }
}
