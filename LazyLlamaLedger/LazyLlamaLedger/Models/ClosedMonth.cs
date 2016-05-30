using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    /// <summary>
    /// Represents a month that is closed
    /// </summary>
    public class ClosedMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
