﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool IsExpense { get; set; }
        
        public int Uses { get; set; }

        public List<SubCategory> Subcats { get; set; }

        public Category()
        {
            Uses = 0;
        }
    }
}
