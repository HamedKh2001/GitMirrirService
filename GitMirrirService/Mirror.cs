using System.Diagnostics;

namespace GitMirrorService
{
    public class Mirror
    {
        //private static string WorkingDirectory;

        public static void StarToMirror(string sourceUrl, string destinationUrl, string WorkingDirectory)
        {
           //var WorkingDirectory = Directory.GetCurrentDirectory();
            WorkingDirectory = Path.Combine(WorkingDirectory, GetMyDateTime());
            if (!Directory.Exists(WorkingDirectory))
                Directory.CreateDirectory(WorkingDirectory);

            CommandExecutor($"git clone --mirror {sourceUrl}", WorkingDirectory);
            Task.Delay(30000).Wait();

            WorkingDirectory = Path.Combine(WorkingDirectory, sourceUrl.Split('/').ToList().Last());
            CommandExecutor($"git remote set-url --push origin {destinationUrl}", WorkingDirectory);
            Task.Delay(5000).Wait();

            CommandExecutor($"git push --mirror", WorkingDirectory);
        }


        static void CommandExecutor(string command, string workingDirectory)
        {
            command = "/c " + command;
            var p = new Process
            {
                StartInfo =
                    {
                        FileName = "C:\\Windows\\system32\\cmd.exe",
                        WorkingDirectory = workingDirectory,
                        Arguments = command
                    }
            };
            p.Start();
        }

        static string GetMyDateTime()
        {
            var dt = DateTime.Now;
            return $"{dt.Year}-{dt.Month}-{dt.Day}~~{dt.Hour}-{dt.Minute}-{dt.Second}";
        }

        //static string ResetWorkingDirectory(string workingDirectory)
        //{
        //    workingDirectory = string.Empty;
        //    return Environment.CurrentDirectory;
        //}
    }
}