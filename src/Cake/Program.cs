using System;
using System.Linq;
using Autofac;
using Cake.Arguments;
using Cake.Autofac;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;

namespace Cake
{
    /// <summary>
    /// The Cake program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <returns>The application exit code.</returns>
        public static int Main()
        {
            ICakeLog log = null;

            try
            {
                // Parse arguments.
                var args = ArgumentTokenizer
                    .Tokenize(Environment.CommandLine)
                    .Skip(1) // Skip executable.
                    .ToArray();

                var builder = new ContainerBuilder();
                builder.RegisterModule<CakeModule>();

                // Build the container.
                using (var container = builder.Build())
                {
                    // Resolve the log.
                    log = container.Resolve<ICakeLog>();

                    // Parse the options.
                    var parser = container.Resolve<IArgumentParser>();
                    var options = parser.Parse(args);

                    // Rebuild the container.
                    builder = new ContainerBuilder();
                    builder.RegisterModule(new ArgumentsModule(options));
                    builder.RegisterModule(new ConfigurationModule(container, options));
                    builder.RegisterModule(new ScriptingModule(options));
                    builder.Update(container);

                    // Resolve and run the application.
                    var application = container.Resolve<CakeApplication>();
                    return application.Run(options);
                }
            }
            catch (Exception ex)
            {
                log = log ?? new CakeBuildLog(new CakeConsole());
                if (log.Verbosity == Verbosity.Diagnostic)
                {
                    log.Error("Error: {0}", ex);
                }
                else
                {
                    log.Error("Error: {0}", ex.Message);
                }
                return 1;
            }
        }
    }
}