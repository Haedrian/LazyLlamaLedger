using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class SubCategory
    {
        public int ID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
