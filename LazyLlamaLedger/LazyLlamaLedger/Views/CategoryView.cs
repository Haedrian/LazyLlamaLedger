using LazyLlamaLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Views
{
    public class CategoryView
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public List<string> Subcategories { get; set; }

        public CategoryView()
        {

        }

        public CategoryView(Category cat, List<SubCategory> subcats)
        {
            this.ID = cat.ID;
            this.CategoryName = cat.Name;

        }

    }
}
