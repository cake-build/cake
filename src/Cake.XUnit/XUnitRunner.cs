using System;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public sealed class XUnitRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _runner;

        public XUnitRunner(ICakeEnvironment environment, IGlobber globber, IProcessRunner runner = null)
        {
            _environment = environment;
            _globber = globber;
            _runner = runner ?? new ProcessRunner();
        }

        public void Run(FilePath assemblyPath)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }

            // Find the xUnit console runner.
            var query = string.Format("./tools/**/xunit.console.clr4.exe");
            var runnerPath = _globber.GetFiles(query).FirstOrDefault();
            if (runnerPath == null)
            {
                throw new CakeException("Could not find xUnit runner.");
            }

            // Get the assemblies to build.
            var assembly = assemblyPath.FullPath.Quote();

            // Create the process start info.
            var info = new ProcessStartInfo(runnerPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
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
