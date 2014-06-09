using System.Diagnostics;
using System.Linq;
using Cake.Core;
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
            var runnerPath = context.Globber.GetFiles(query).FirstOrDefault();
            if (runnerPath == null)
            {
                throw new CakeException("Could not find xUnit runner.");
            }

            // Get the assemblies to build.
            var assembly = string.Concat("\"", settings.Assembly.FullPath, "\"");

            // Create the process start info.
            var info = new ProcessStartInfo(runnerPath.FullPath)
            {
                WorkingDirectory = context.Environment.WorkingDirectory.FullPath,
                Arguments = assembly,
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
