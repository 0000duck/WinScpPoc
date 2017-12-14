using System;
using System.IO;
using WinSCP;

namespace FtpClient
{
    internal class Program
    {
        private static string remotePath = "/sample";

        private static string filename = $"test-{DateTime.Now.ToString("yyyyddMMhhmmss")}.txt";

        private static void Main(string[] args)
        {
            // Set up session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "localhost",
                PortNumber = 21,
                UserName = "pradeep",
                Password = "pradeep",
                // Enable FTPS in explicit mode, aka FTPES
                FtpSecure = FtpSecure.None
            };

            using (Session session = new Session())
            {
                // Connect
                session.Open(sessionOptions);

                //Delete directory
                DeleteDirectory(session);

                //Create directory
                CreateDirectory(session);

                //Upload file
                UploadFile(session);

                //List directory content
                DisplayDirectoryContents(session);

                //Rename file
                RenameFile(session);

                //List directory content
                DisplayDirectoryContents(session);

                //Download file
                DownloadFile(session);

                //Delete file
                DeleteFile(session);
            }
        }

        private static void DownloadFile(Session session)
        {
            TransferOptions transferOptions = new TransferOptions();
            transferOptions.TransferMode = TransferMode.Binary;

            session.GetFiles($"{remotePath}/New-{filename}", $"New-{filename}", options: transferOptions);
        }

        private static void DeleteDirectory(Session session)
        {
            session.RemoveFiles(remotePath);
        }

        private static void DeleteFile(Session session)
        {
            session.RemoveFiles($"{remotePath}/*.*");
        }

        private static void CreateDirectory(Session session)
        {
            session.CreateDirectory(remotePath);
        }

        private static void RenameFile(Session session)
        {
            session.MoveFile($"{remotePath}/{filename}", $"{remotePath}/New-{filename}");
        }

        private static void DisplayDirectoryContents(Session session)
        {
            var directoryInfo = session.ListDirectory(remotePath);

            Console.WriteLine("Printing file list");
            var files = directoryInfo.Files;
            foreach (var file in files)
            {
                Console.WriteLine($"{file.ToString()}");
            }
        }

        private static void UploadFile(Session session)
        {
            string content = "Hello World!!!";

            File.WriteAllText(filename, content);

            TransferOptions transferOptions = new TransferOptions();
            transferOptions.TransferMode = TransferMode.Binary;

            session.PutFiles(filename, $"{remotePath}/{filename}", options: transferOptions);
        }
    }
}