using LazyLlamaLedger.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Views
{
    public class CategoryView
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<string> Subcategories { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool IsExpense { get; set; }

        public CategoryView()
        {

        }

        public CategoryView(Category cat, List<SubCategory> subcats)
        {
            this.ID = cat.ID;
            this.Name = cat.Name;
            this.Subcategories = subcats.Where(s => s.Active).Select(s => s.Name).ToList();
            this.Active = cat.Active;
            this.IsExpense = cat.IsExpense;
        }

    }
}
