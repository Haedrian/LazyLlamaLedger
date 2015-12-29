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
using System.Configuration;
using Microsoft.Win32;

namespace LLLApplication
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {


            //Flush everything
            LazyLlamaLedger.DataHandling.FlushCats();
            LazyLlamaLedger.DataHandling.FlushLedgers();


        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Locate the setting file
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

            key = key.OpenSubKey("LazyLlamaLedger", true);
            
            if (key == null)
            {
                //First time being loaded, get user to choose save directory
                FolderBrowserDialog fbd = new FolderBrowserDialog()
                {
                    Description = "Choose location to save data files",
                    SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    ShowNewFolderButton = true
                };

                //This is the default folder, if they press cancel, we'll choose for them
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/LazyLlamaLedger";

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    directory = fbd.SelectedPath;
                }

                key = Registry.CurrentUser.OpenSubKey("Software", true);
                key = key.CreateSubKey("LazyLlamaLedger");

                //Write it to the key
                key.SetValue("DataPath", directory);

                DataHandling.FolderPath = directory;
            }            
            else
            {
                //Read it off the key
                DataHandling.FolderPath = key.GetValue("DataPath").ToString();
            }

            Directory.CreateDirectory(DataHandling.FolderPath); //Create the directory

            DataHandling.StartDataHandling();

            //We're done? Now we can start
            string baseAddress = "http://localhost:7744/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);

            //Open the browser
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings["PagePath"]);
        }

        private void tmrFlush_Tick(object sender, EventArgs e)
        {
            //Flush everything - use a task so we don't block anything
            Task.Factory.StartNew(() => DataHandling.FlushAll());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                //notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }
    }
}
