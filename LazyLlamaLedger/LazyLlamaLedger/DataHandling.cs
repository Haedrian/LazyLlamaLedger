using LazyLlamaLedger.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger
{
    public static class DataHandling
    {
        public static List<LedgerEntry> LedgerEntries { get; set; }
        public static List<Category> Categories { get; set; }
        public static List<SubCategory> SubCategories { get; set; }

        public static void AddLedgerEntry(LedgerEntry le)
        {
            le.ID = LedgerEntries.Count + 1;

            LedgerEntries.Add(le);

            FlushLedgers();
        }

        public static void AddCategory(Category cat)
        {
            cat.ID = Categories.Count + 1;

            Categories.Add(cat);

            FlushCats();
        }

        public static void AddSubCategory(SubCategory subCat)
        {
            subCat.ID = SubCategories.Count + 1;
            SubCategories.Add(subCat);

            FlushSubcats();
        }

        static DataHandling()
        {
            LedgerEntries = new List<LedgerEntry>();
            Categories = new List<Category>();
            SubCategories = new List<SubCategory>();

            if (File.Exists("les.json"))
            {
                string les = File.ReadAllText("les.json");
                var fromFile = JsonConvert.DeserializeObject<List<LedgerEntry>>(les);

                if (fromFile != null)
                {
                    LedgerEntries.AddRange(fromFile);
                }
            }
            
            if (File.Exists("cats.json"))
            {
                string cats = File.ReadAllText("cats.json");
                var fromFile = JsonConvert.DeserializeObject<List<Category>>(cats);

                if (fromFile != null)
                {
                    Categories.AddRange(fromFile);
                }
            }
            
            if (File.Exists("subcats.json"))
            {
                string subcats = File.ReadAllText("subcats.json");
                var fromFile = JsonConvert.DeserializeObject<List<SubCategory>>(subcats);

                if (fromFile != null)
                {
                    SubCategories.AddRange(fromFile);
                }
            }
            
        }

        public static void FlushLedgers()
        {
            string les = JsonConvert.SerializeObject(LedgerEntries);
            File.WriteAllText("les.json", les);
        }

        public static void FlushCats()
        {
            string cats = JsonConvert.SerializeObject(Categories);
            File.WriteAllText("cats.json", cats);   
        }

        public static void FlushSubcats()
        {
            string subCats = JsonConvert.SerializeObject(SubCategories);
            File.WriteAllText("subcats.json", subCats);
        }
        
    }
}
