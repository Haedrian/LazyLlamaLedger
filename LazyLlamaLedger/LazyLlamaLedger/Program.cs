using LazyLlamaLedger.Models;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LazyLlamaLedger
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:7744/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);

            Console.WriteLine("Started Service on " + baseAddress);

            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());

            Console.ReadLine();
        }
    }
}
