using System.Diagnostics;

namespace GitMirrirService
{
    public class Mirror
    {
        static string SourceUrl = string.Empty;
        static string DestinationUrl = string.Empty;
        static string WorkingDirectory = Environment.CurrentDirectory;
        public static void StarToMirror()
        {
            Console.WriteLine($"Enter Source Git Url?");
            SourceUrl = InputValidator(Console.ReadLine());

            CommandExecutor($"git clone --mirror {SourceUrl}", WorkingDirectory);
            Console.WriteLine($"Enter Destination Git Url?");
            DestinationUrl = InputValidator(Console.ReadLine());
            WorkingDirectory = Path.Combine(WorkingDirectory, SourceUrl.Split('/').ToList().Last());
            CommandExecutor($"git remote set-url --push origin {DestinationUrl}", WorkingDirectory);
            Task.Delay(10000).Wait();
            CommandExecutor($"git push --mirror", WorkingDirectory);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Mirroring is Done :)");
            Console.ReadLine();
        }


        static string InputValidator(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid Input...Try Again...");
                Console.ForegroundColor = ConsoleColor.White;
                InputValidator(Console.ReadLine());
            }
            return string.Empty;
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
    }
}