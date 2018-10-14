using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WinSCP;

namespace ServiceExample
{
    public class FTPTransfer
    {
        protected string HostName { get; set; }
        protected string UserName { get; set; }
        protected string Password { get; set; }
        protected string remoteDir { get; set; }
        protected string localDestnDir { get; set; }

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

        public FTPTransfer(string hostName, string userName, string password, string remoteDir, string localDir)
        {
            this.HostName = hostName;
            this.UserName = userName;
            this.Password = password;
            this.remoteDir = remoteDir;
            this.localDestnDir = localDir;
        }

        public void GetFilesFromFtp()
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    // Protocol = protocol.Sftp
                    Protocol = Protocol.Ftp,
                    HostName = HostName,
                    UserName = UserName,
                    Password = Password
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Download files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult =
                        session.GetFiles(remoteDir, @localDestnDir, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        string status = "Download of "+ transfer.FileName + "succeeded at " + DateTime.Now;
                        LogService(status);
                    }
                }

                
            }
            catch (Exception e)
            {
                string status = DateTime.Now + " - Error: " + e;
                LogService(status);
                
            }
        }
    }
}
      