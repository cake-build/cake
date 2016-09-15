// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// The NUnit unit test runner.
    /// </summary>
    public sealed class NUnitRunner : Tool<NUnitSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public NUnitRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assemblies, using the specified settings.
        /// </summary>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, NUnitSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(assemblyPaths, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, NUnitSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assemblies to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Framework != null)
            {
                builder.AppendQuoted("-framework:" + settings.Framework);
            }

            if (settings.Include != null)
            {
                builder.AppendQuoted("-include:" + settings.Include);
            }

            if (settings.Exclude != null)
            {
                builder.AppendQuoted("-exclude:" + settings.Exclude);
            }

            if (settings.Timeout.HasValue)
            {
                builder.Append("-timeout:" + settings.Timeout.Value);
            }

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.Append("-noshadow");
            }

            if (settings.NoLogo)
            {
                builder.Append("-nologo");
            }

            if (settings.NoThread)
            {
                builder.Append("-nothread");
            }

            if (settings.StopOnError)
            {
                builder.Append("-stoponerror");
            }

            if (settings.Trace != null)
            {
                builder.Append("-trace:" + settings.Trace);
            }

            if (settings.OutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "-output:{0}", settings.OutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.ErrorOutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "-err:{0}", settings.ErrorOutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.ResultsFile != null && settings.NoResults)
            {
                throw new ArgumentException(
                    GetToolName() + ": You can't specify both a results file and set NoResults to true.");
            }

            if (settings.ResultsFile != null)
            {
                builder.AppendQuoted(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "-result:{0}", settings.ResultsFile.MakeAbsolute(_environment).FullPath));
            }
            else if (settings.NoResults)
            {
                builder.AppendQuoted("-noresult");
            }

            // don't include the default value
            if (settings.Process != NUnitProcessOption.Single)
            {
                builder.AppendQuoted("-process:" + settings.Process);
            }

            if (settings.UseSingleThreadedApartment)
            {
                builder.AppendQuoted("-apartment:STA");
            }

            if (settings.AppDomainUsage != NUnitAppDomainUsage.Default)
            {
                builder.AppendQuoted("-domain:" + settings.AppDomainUsage);
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "NUnit";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "nunit-console.exe" };
        }

        /// <summary>
        /// Customized exit code handling.
        /// Throws <see cref="CakeException"/> on non-zero exit code
        /// </summary>
        /// <param name="exitCode">The process exit code</param>
        protected override void ProcessExitCode(int exitCode)
        {
            string error;

            if (exitCode <= 0)
            {
                switch (exitCode)
                {
                    case 0:
                        return;
                    case -1:
                        error = "Invalid argument";
                        break;
                    case -2:
                        error = "File not found";
                        break;
                    case -3:
                        error = "Test fixture not found";
                        break;
                    case -100:
                        error = "Unexpected error";
                        break;
                    default:
                        error = "Unrecognised error";
                        break;
                }
            }
            else
            {
                error = string.Format(CultureInfo.InvariantCulture, "{0} test(s) failed", exitCode);
            }

            const string message = "{0}: {1} (exit code {2}).";
            throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, GetToolName(), error, exitCode));
        }
    }
}