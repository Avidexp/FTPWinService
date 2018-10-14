using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceExample
{
    public partial class TestService : ServiceBase
    {
        private System.Timers.Timer timeDelay;
        private int count;
        private DateTime currentDate;
        private string logMessage;
        public TestService()
        {
            InitializeComponent();
            timeDelay = new System.Timers.Timer();
            timeDelay.Elapsed += new System.Timers.ElapsedEventHandler(WorkProcess);

        }

        public void WorkProcess(object sender, System.Timers.ElapsedEventArgs e)
        {

            FTPTransfer FtpClient = new FTPTransfer("host", "username", "password", "/", "C:/TestFiles");
            FtpClient.GetFilesFromFtp();
        }

        protected override void OnStart(string[] args)
        {
            currentDate = System.DateTime.Now;
            logMessage = "Service Started - " + currentDate;
            LogService(logMessage);
            timeDelay.Enabled = true;
        }

        protected override void OnStop()
        {
            currentDate = System.DateTime.Now;
            logMessage = "Service Stopped - " + currentDate;
            LogService(logMessage);
            timeDelay.Enabled = false;

        }

        // Writes status to file
        private void LogService(string content)
        {
            FileStream fs =
                new FileStream(@"C:\Users\dj075\Documents\C#Reference\WindowsService\ServiceExample\Logs.txt",
                    FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }

    }
}
