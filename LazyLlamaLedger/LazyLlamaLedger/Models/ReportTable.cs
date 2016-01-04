using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class ReportTable
    {
        public List<ReportRow> Rows;
        public List<string> Headers;
    }

    public class ReportRow
    {
        public string CategoryName;
        public List<string> Values;

        public int CatID { get; internal set; }
    }
}
