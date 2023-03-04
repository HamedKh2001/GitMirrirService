using System.Diagnostics;

namespace GitMirrorService
{
    public class Mirror
    {
        private static string WorkingDirectory = Environment.CurrentDirectory;

        public static void StarToMirror(string sourceUrl, string destinationUrl)
        {
            WorkingDirectory = Path.Combine(WorkingDirectory, GetMyDateTime());
            CommandExecutor($"git clone --mirror {sourceUrl}", WorkingDirectory);
            WorkingDirectory = Path.Combine(WorkingDirectory, sourceUrl.Split('/').ToList().Last());
            CommandExecutor($"git remote set-url --push origin {destinationUrl}", WorkingDirectory);
            Task.Delay(25000).Wait();
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
            return $"{dt.Year}-{dt.Month}-{dt.Day}~{dt.Hour}:{dt.Minute}:{dt.Second}";
        }
    }
}