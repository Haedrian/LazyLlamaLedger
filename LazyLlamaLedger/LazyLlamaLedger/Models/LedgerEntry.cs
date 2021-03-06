﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;

namespace LazyLlamaLedger.Models
{
    public class LedgerEntry
    {
        public int ID { get; set; }
        [Required]
        public string Item { get; set; }
        [Required]
        public bool IsExpense { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Category { get; set; }
        [Required]
        public int SubCategory { get; set; }
        [Required]
        public decimal Money { get; set; }

        public bool IsCollective { get; set; }

        public string Fund { get; set; }

        public LedgerEntry()
        {
            IsCollective = false;
        }
    }
}
