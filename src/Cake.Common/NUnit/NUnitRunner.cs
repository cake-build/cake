using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Extensions;

namespace Cake.Common.NUnit
{
    public sealed class NUnitRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        public NUnitRunner(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        public void Run(FilePath assemblyPath, NUnitSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }

            // Find the NUnit console runner.
            var toolPath = GetToolPath(settings);

            // Get the process start info.
            var info = GetProcessStartInfo(assemblyPath, settings, toolPath);

            // Run the process.
            var process = _processRunner.Start(info);
            if (process == null)
            {
                throw new CakeException("NUnit process was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Failing NUnit tests.");
            }
        }

        private FilePath GetToolPath(NUnitSettings settings)
        {
            if (settings.ToolPath != null)
            {
                return settings.ToolPath.MakeAbsolute(_environment);
            }

            var expression = string.Format("./tools/**/nunit-console.exe");
            var runnerPath = _globber.GetFiles(expression).FirstOrDefault();
            if (runnerPath == null)
            {
                throw new CakeException("Could not find nunit-console.exe.");
            }
            return runnerPath;
        }

        private ProcessStartInfo GetProcessStartInfo(FilePath assemblyPath, NUnitSettings settings, FilePath runnerPath)
        {
            var parameters = new List<string>();

            // Add the assembly to build.
            parameters.Add(assemblyPath.MakeAbsolute(_environment).FullPath.Quote());

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                parameters.Add("/noshadow".Quote());
            }

            // Create the process start info.
            return new ProcessStartInfo(runnerPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
