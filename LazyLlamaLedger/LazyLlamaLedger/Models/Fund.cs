using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class Fund
    {
        /// <summary>
        /// Name of the Fund
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Colour to Display Fund in
        /// </summary>
        [Required]
        public string Colour { get; set; }

        /// <summary>
        /// Minimum Amount to transfer to this fund every month
        /// </summary>
        public decimal? MinimumAmount { get; set; }
        /// <summary>
        /// Maximum Amount to transfer to this fund every month
        /// </summary>
        public decimal? MaximumAmount { get; set; }
        /// <summary>
        /// The percentage amount to transfer to this fund every month
        /// </summary>
        public decimal? Percentage { get; set; }
        /// <summary>
        /// If this month made no profit - determines if we get the minimum amount, or nothing at all
        /// </summary>
        public bool MinimumIfNegative { get; set; }

        public decimal Total { get; set; }

        public bool IsActive { get; set; }

        public Fund()
        {
            MinimumAmount = MaximumAmount = Percentage = null;
            Total = 0;
            MinimumIfNegative = false;
            IsActive = true;
        }

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
