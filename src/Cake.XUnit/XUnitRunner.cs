using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public sealed class XUnitRunner
    {
        private readonly IProcessRunner _runner;

        public XUnitRunner(IProcessRunner runner = null)
        {
            _runner = runner ?? new ProcessRunner();
        }

        public void Run(ICakeContext context, XUnitSettings settings)
        {
            // Find the xUnit console runner.
            var query = string.Format("./tools/**/xunit.console.clr4.exe");
            var runnerPath = context.GetFiles(query).FirstOrDefault();
            if (runnerPath == null)
            {
                throw new CakeException("Could not find xUnit runner.");
            }

            // Get the assemblies to build.
            var assemblies = settings.GetAssemblyPaths().ToArray();
            if (assemblies.Length == 0)
            {
                throw new CakeException("No assembly paths specified.");
            }
            var parameters = assemblies.Select(x => string.Concat("\"", x.FullPath, "\"")).ToArray();

            // Create the process start info.
            var info = new ProcessStartInfo(runnerPath.FullPath)
            {
                WorkingDirectory = context.Environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };

            // Run the process.
            var process = _runner.Start(info);
            if (process == null)
            {
                throw new CakeException("xUnit process was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Failing xUnit tests.");
            }
        }
    }
}
