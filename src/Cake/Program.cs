using System;
using Cake.Arguments;
using Cake.Bootstrapping;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Diagnostics;
using Cake.Scripting;

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
                var application = CreateApplication(log);
                application.Run(options);

                return 0;
            }
            catch (Exception ex)
            {
                log.Error("Error: {0}", ex.Message);
                return 1;
            }
        }

        private static CakeApplication CreateApplication(ICakeLog log)
        {
            var fileSystem = new FileSystem();
            var environment = new CakeEnvironment();
            var bootstrapper = new CakeBootstrapper(fileSystem, log, new NuGetInstaller(fileSystem, log));
            var scriptRunner = new ScriptRunner(log, new RoslynScriptSessionFactory());
            return new CakeApplication(bootstrapper, fileSystem, environment, log, scriptRunner);
        }
    }
}
