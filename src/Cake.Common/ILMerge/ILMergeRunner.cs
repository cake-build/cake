using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.Common.ILMerge
{
    public sealed class ILMergeRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        public ILMergeRunner(ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
        {
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
        }

        public void Merge(FilePath outputAssemblyPath, FilePath primaryAssemblyPath, 
            IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings = null)
        {
            if (outputAssemblyPath == null)
            {
                throw new ArgumentNullException("outputAssemblyPath");
            }
            if (primaryAssemblyPath == null)
            {
                throw new ArgumentNullException("primaryAssemblyPath");
            }
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }

            settings = settings ?? new ILMergeSettings();

            // Get the ILMerge path.
            var toolPath = GetToolPath(settings);

            // Get the process start info.
            var info = GetProcessStartInfo(toolPath, outputAssemblyPath, primaryAssemblyPath, assemblyPaths, settings);

            // Run the process.
            var process = _processRunner.Start(info);
            if (process == null)
            {
                throw new CakeException("ILMerge process was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("Failed to merge assemblies.");
            }
        }

        private ProcessStartInfo GetProcessStartInfo(FilePath toolPath, FilePath outputAssemblyPath,
            FilePath primaryAssemblyFilePath, IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings)
        {
            var parameters = new List<string>();

            parameters.Add(GetOutputParameter(outputAssemblyPath));

            if (settings.TargetKind != TargetKind.Default)
            {
                parameters.Add(GetTargetKindParameter(settings));
            }

            if (settings.Internalize)
            {
                parameters.Add("/internalize");
            }

            // Add primary assembly.
            parameters.Add(primaryAssemblyFilePath.MakeAbsolute(_environment).FullPath.Quote());

            foreach (var file in assemblyPaths)
            {
                parameters.Add(file.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Create the process start info.
            return new ProcessStartInfo(toolPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }

        private FilePath GetToolPath(ILMergeSettings settings)
        {
            if (settings.ToolPath != null)
            {
                return settings.ToolPath.MakeAbsolute(_environment);
            }
            var expression = string.Format("./tools/**/ILMerge.exe");
            var nugetExePath = _globber.GetFiles(expression).FirstOrDefault();
            if (nugetExePath == null)
            {
                throw new CakeException("Could not find ILMerge.exe.");
            }
            return nugetExePath;
        }

        private string GetOutputParameter(FilePath outputAssemblyPath)
        {
            var path = outputAssemblyPath.MakeAbsolute(_environment);
            return string.Concat("/out:", path.FullPath.Quote());
        }

        private static string GetTargetKindParameter(ILMergeSettings settings)
        {
            return string.Concat("/target:", GetTargetKindName(settings.TargetKind).Quote());
        }

        private static string GetTargetKindName(TargetKind kind)
        {
            switch (kind)
            {
                case TargetKind.Dll:
                    return "dll";
                case TargetKind.Exe:
                    return "exe";
                case TargetKind.WinExe:
                    return "winexe";
                default:
                    throw new NotSupportedException("The provided ILMerge target kind is not valid.");
            }
        }
    }
}
