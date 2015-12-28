using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;
using LazyLlamaLedger;
using System.IO;

namespace LLLApplication
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            AddToLog("Starting Lazy Llama Ledger");

            string baseAddress = "http://localhost:7744/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);

            AddToLog("Started Service on " + baseAddress);

            //Open the browser
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "/../../../WebApp/LazyLlamaLedger.htm"); //TODO: Make this app.config-able
        }

        /// <summary>
        /// Displays a log message to the user
        /// </summary>
        /// <param name="log"></param>
        public void AddToLog(string log)
        {
            txtLog.Text = log + "\r\n" + txtLog.Text;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AddToLog("Please wait while we save the files");

            //Flush everything
            LazyLlamaLedger.DataHandling.FlushCats();
            LazyLlamaLedger.DataHandling.FlushLedgers();

            AddToLog("Done. Bye");
        }
    }
}
