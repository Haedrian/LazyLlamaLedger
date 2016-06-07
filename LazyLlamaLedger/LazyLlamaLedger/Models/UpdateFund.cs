using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class UpdateFund
    {
        public decimal? MinimumAmount { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? MaximumAmount { get; set; }

        public bool MinimumIfNegative { get; set; }
        public bool IsActive { get; set; }

        public bool CheckIfValid(out string issue)
        {
            if (!MinimumAmount.HasValue && !Percentage.HasValue)
            {
                issue = "A fund must have a minimum amount or a percentage";
                return false;
            }
            else if (MinimumAmount.HasValue && MaximumAmount.HasValue && MinimumAmount > MaximumAmount)
            {
                issue = "Minimum Amount must be smaller than Maximum Amount";
                return false;
            }

            issue = String.Empty;
            return true;
        }
    }
}
