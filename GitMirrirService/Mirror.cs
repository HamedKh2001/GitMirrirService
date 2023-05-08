using System.Diagnostics;

namespace GitMirrorService
{
    public class Mirror
    {
        public static void StarToMirror(string sourceUrl, string destinationUrl, string WorkingDirectory, ILogger logger)
        {
            try
            {
                WorkingDirectory = Path.Combine(WorkingDirectory, GetMyDateTime());
                if (!Directory.Exists(WorkingDirectory))
                    Directory.CreateDirectory(WorkingDirectory);

                CommandExecutor($"git clone --mirror {sourceUrl}", WorkingDirectory);
                logger.LogInformation("git clone done.");

                WorkingDirectory = Path.Combine(WorkingDirectory, sourceUrl.Split('/').ToList().Last());
                CommandExecutor($"git remote set-url --push origin {destinationUrl}", WorkingDirectory);

                CommandExecutor($"git push --mirror", WorkingDirectory);
                logger.LogInformation("pushed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
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
            p.WaitForExit();
        }

        static string GetMyDateTime()
        {
            var dt = DateTime.Now;
            return $"{dt.Year}-{dt.Month}-{dt.Day}~~{dt.Hour}-{dt.Minute}-{dt.Second}";
        }
    }
}