using System;
using Cake.Arguments;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;

namespace Cake
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var log = new ColoredConsoleBuildLog();

            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Usage: Cake.exe <script> [-verbosity=value] [scriptarguments]");
                    return 0;
                }

                // Parse arguments.
                var parser = new ArgumentParser(log);
                var options = parser.Parse(args);

                // Create and run the application.
                var application = new CakeApplication(log: log);
                application.Run(options);

                return 0;
            }
            catch (Exception ex)
            {
                log.Error("Error: {0}", ex.Message);
                return 1;
            }
        }
    }
}
