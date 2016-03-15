using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class CollectiveLedgerEntry
    {
        public int ID { get; set; }
        [Required]
        public string Item { get; set; }
        [Required]
        public bool IsExpense { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public int Category { get; set; }
        [Required]
        public int SubCategory { get; set; }
        [Required]
        public decimal Money { get; set; }
    }
}
