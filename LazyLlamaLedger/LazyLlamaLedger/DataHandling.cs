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
        public static string FolderPath = "";

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

        /// <summary>
        /// This starts the data handler
        /// Had to be done this way, because we can't guarantee when the constructor starts
        /// </summary>
        public static void StartDataHandling()
        {
            LedgerEntries = new List<LedgerEntry>();
            Categories = new List<Category>();

            //TODO: DEFAULT DATA
            if (File.Exists(FolderPath + Path.DirectorySeparatorChar + "les.json"))
            {
                string les = File.ReadAllText(FolderPath + Path.DirectorySeparatorChar + "les.json");
                var fromFile = JsonConvert.DeserializeObject<List<LedgerEntry>>(les);

                if (fromFile != null)
                {
                    LedgerEntries.AddRange(fromFile);
                }
            }

            if (File.Exists(FolderPath + Path.DirectorySeparatorChar + "cats.json"))
            {
                string cats = File.ReadAllText(FolderPath + Path.DirectorySeparatorChar + "cats.json");
                var fromFile = JsonConvert.DeserializeObject<List<Category>>(cats);

                if (fromFile != null)
                {
                    Categories.AddRange(fromFile);
                }
            }
        }

        static DataHandling()
        {
            

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
                File.WriteAllText(FolderPath + Path.DirectorySeparatorChar + "les.json", les);
            }
        }

        public static void FlushCats()
        {
            lock(fileLock)
            {
                string cats = JsonConvert.SerializeObject(Categories);
                File.WriteAllText(FolderPath + Path.DirectorySeparatorChar + "cats.json", cats);
            }
        }

        public static void FlushAll()
        {
            lock(fileLock)
            {
                string les = JsonConvert.SerializeObject(LedgerEntries);
                File.WriteAllText(FolderPath + Path.DirectorySeparatorChar + "les.json", les);

                string cats = JsonConvert.SerializeObject(Categories);
                File.WriteAllText(FolderPath + Path.DirectorySeparatorChar + "cats.json", cats);
            }
        }
    }
}
