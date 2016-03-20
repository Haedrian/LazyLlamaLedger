﻿using LazyLlamaLedger.Models;
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
        /// <summary>
        /// This is to fix a bug with the dirty fix for the relative shortcut. I'll have to pass this as a param
        /// </summary>
        public static string relativeChange = "";

        public static bool isFirstTime = false;
        public static string FolderPath = "";

        private static List<MonthYearPair> LoadedFiles { get; set; }
        private static List<MonthYearPair> DirtyFiles { get; set; }
        private static List<LedgerEntry> Entries { get; set; }

        public static List<Category> Categories { get; set; }


        private static List<LedgerEntry> LoadEntriesFromFile(DateTime yearMonth)
        {
            string path = FolderPath + Path.DirectorySeparatorChar + "les" + yearMonth.ToString("MMyyyy") + ".json";

            if (File.Exists(path))
            {
                string les = File.ReadAllText(path);
                var fromFile = JsonConvert.DeserializeObject<List<LedgerEntry>>(les);

                if (fromFile != null)
                {
                    return fromFile;    
                }
                else
                {
                    return new List<LedgerEntry>();
                }
            }
            else
            {
                return new List<LedgerEntry>(); //Nothing to load
            }
        }

        public static List<LedgerEntry> GetLedgerEntries(int year, int month)
        {
            return GetLedgerEntries(new DateTime(year, month, 1), new DateTime(year, month, 1));
        }

        public static List<LedgerEntry> GetLedgerEntries(DateTime monthFrom,DateTime monthTo)
        {
            //Have we loaded each file ?
            DateTime timeCursor = monthFrom;

            while (timeCursor <= monthTo)
            {
                if (!LoadedFiles.Any(lf => lf.Month == timeCursor.Month && lf.Year == timeCursor.Year))
                {
                    //Not loaded yet, lets do so
                    Entries.AddRange(LoadEntriesFromFile(timeCursor));

                    //Order the entries
                    Entries = Entries.OrderBy(e => e.Date).ToList();

                    //Loaded them
                    LoadedFiles.Add(new MonthYearPair(timeCursor));
                }

                timeCursor = timeCursor.AddMonths(1);
            }

            //Now we can return the ledger entries as requested
            return Entries.Where(e => e.Date > monthFrom && e.Date < monthTo).ToList();
        }

        public static void AddLedgerEntry(LedgerEntry le)
        {
            lock(fileLock) //This is to prevent a race condition to do with the dirty files
            {
                //Have we loaded the correct file already?
                if (!LoadedFiles.Any(df => df.Month == le.Date.Month && df.Year == le.Date.Year))
                {
                    //Good, no need to load them
                    //But we've dirtied the file, so lets mark accordingly
                    if (!DirtyFiles.Any(df => df.Month == le.Date.Month && df.Year == le.Date.Year))
                    {
                        DirtyFiles.Add(new MonthYearPair(le)); //Mark
                    }
                }
                Entries.Add(le);
            }
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
            Entries = new List<LedgerEntry>();
            Categories = new List<Category>();
            DirtyFiles = new List<MonthYearPair>();
            LoadedFiles = new List<MonthYearPair>();

            //We'll load the entries lazily as we need them

            //Does he still have from the old file format? Better read it, then delete it - we'll convert it to the new format for him :)
            if (File.Exists(FolderPath + Path.DirectorySeparatorChar + "les.json"))
            {
                string les = File.ReadAllText(FolderPath + Path.DirectorySeparatorChar + "les.json");
                var fromFile = JsonConvert.DeserializeObject<List<LedgerEntry>>(les);

                if (fromFile != null)
                {
                    Entries.AddRange(fromFile);

                    //Go through them and pick out the unique month-years
                    MonthYearPair[] uniqueMonths = Entries.Select(e => new MonthYearPair(e)).Distinct().ToArray();

                    DirtyFiles.AddRange(uniqueMonths);
                    LoadedFiles.AddRange(uniqueMonths);

                    //Flush
                    FlushLedgers();

                    //And delete - no more
                    File.Delete(FolderPath + Path.DirectorySeparatorChar + "les.json");
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
            else
            {
                DataHandling.isFirstTime = true;

                //Put in a default pair of cats
                Categories.Add(new Category()
                {
                    Active = true,
                    ID = 0,
                    IsExpense = true,
                    Name = "Misc Expenditure",
                    Subcats = new List<SubCategory>()
                    {
                        new SubCategory()
                        {
                            Active = true,
                            ID = 0,
                            Name = "Other"
                        }
                    }
                });

                Categories.Add(new Category()
                {
                    Active = true,
                    ID = 1,
                    IsExpense = false,
                    Name = "Misc Income",
                    Subcats = new List<SubCategory>()
                    {
                        new SubCategory()
                        {
                            Active = true,
                            ID = 0,
                            Name = "Other"
                        }
                    }
                });

            }
        }

        public static void LoadCategoryStarterPack()
        {
            //Load the file from resources
            string sPack = File.ReadAllText(String.IsNullOrEmpty(relativeChange) ? "./Resources/cats.json" : "./" + relativeChange + "/" + "Resources/cats.json");

            List<Category> sPackCats = JsonConvert.DeserializeObject<List<Category>>(sPack);

            //Go through the active ones one at a time, if one with the same name doesn't exist, then add it
            foreach (var cat in sPackCats.Where(c => c.Active))
            {
                if (!Categories.Any(cc => String.Equals(cc.Name, cat.Name)))
                {
                    AddCategory(cat);
                }
            }
        }

        /// <summary>
        /// So we ensure we're not trying to write to the file at the same time
        /// </summary>
        private static object fileLock = new object();


        public static void FlushLedgers()
        {
            lock (fileLock)
            {
                foreach (var dirty in DirtyFiles)
                {
                    string les = JsonConvert.SerializeObject(Entries.Where(e => e.Date.Year == dirty.Year && e.Date.Month == dirty.Month).ToList());

                    File.WriteAllText(FolderPath + Path.DirectorySeparatorChar + "les "+ dirty.Month+dirty.Year +".json", les);
                }

                DirtyFiles.Clear(); //and clear
            }
        }

        public static void FlushCats()
        {
            lock (fileLock)
            {
                string cats = JsonConvert.SerializeObject(Categories);
                File.WriteAllText(FolderPath + Path.DirectorySeparatorChar + "cats.json", cats);
            }
        }

        public static void FlushAll()
        {
            lock (fileLock)
            {
                FlushLedgers();

                FlushCats();
            }
        }
    }
}
