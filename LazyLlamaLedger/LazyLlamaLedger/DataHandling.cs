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

        public static void AddLedgerEntry(LedgerEntry le)
        {
            le.ID = LedgerEntries.Count + 1;

            LedgerEntries.Add(le);
        }

        public static void AddCategory(Category cat)
        {
            cat.ID = Categories.Count + 1;

            Categories.Add(cat);
        }

        static DataHandling()
        {
            LedgerEntries = new List<LedgerEntry>();
            Categories = new List<Category>();

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

        }

        /// <summary>
        /// So we ensure we're not trying to write to the file at the same time
        /// </summary>
        private static object fileLock = new object();


        public static void FlushLedgers()
        {
            lock(fileLock)
            {
                string les = JsonConvert.SerializeObject(LedgerEntries);
                File.WriteAllText("les.json", les);
            }
        }

        public static void FlushCats()
        {
            lock(fileLock)
            {
                string cats = JsonConvert.SerializeObject(Categories);
                File.WriteAllText("cats.json", cats);
            }
        }

        public static void FlushAll()
        {
            lock(fileLock)
            {
                string les = JsonConvert.SerializeObject(LedgerEntries);
                File.WriteAllText("les.json", les);

                string cats = JsonConvert.SerializeObject(Categories);
                File.WriteAllText("cats.json", cats);
            }
        }
    }
}
