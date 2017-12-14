using System;
using System.IO;
using WinSCP;

namespace SftpClient
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
                Protocol = Protocol.Sftp,
                HostName = "localhost",
                PortNumber = 22,
                UserName = "tester",
                Password = "password",
                SshHostKeyFingerprint = "ssh-rsa 2048 dc:61:65:e7:82:f9:38:8e:fe:f8:ed:18:c5:05:2e:ca"
            };

            using (Session session = new Session())
            {
                // Connect
                session.Open(sessionOptions);

                //Delete directory
                DeleteDirectory(session);

                //Create directory
                CreateDirectory(session);

                //Upload a file
                UploadFile(session);

                //List directory content
                DisplayDirectoryContents(session);

                //Rename a file
                RenameFile(session);

                //List directory content
                DisplayDirectoryContents(session);

                //Download file
                DownloadFile(session);

                //Delete a file
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