// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// The NUnit unit test runner.
    /// </summary>
    public sealed class NUnit3Runner : Tool<NUnit3Settings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnit3Runner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public NUnit3Runner(
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
        public void Run(IEnumerable<FilePath> assemblyPaths, NUnit3Settings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(assemblyPaths, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, NUnit3Settings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assemblies to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Test != null)
            {
                builder.Append("--test=" + settings.Test);
            }

            if (settings.TestList != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "--testlist={0}", settings.TestList.MakeAbsolute(_environment).FullPath));
            }

            if (settings.Where != null)
            {
                builder.Append("--where \"" + settings.Where + "\"");
            }

            if (settings.Timeout.HasValue)
            {
                builder.Append("--timeout=" + settings.Timeout.Value);
            }

            if (settings.Seed.HasValue)
            {
                builder.Append("--seed=" + settings.Seed.Value);
            }

            if (settings.Workers.HasValue)
            {
                builder.Append("--workers=" + settings.Workers.Value);
            }

            if (settings.StopOnError)
            {
                builder.Append("--stoponerror");
            }

            if (settings.Work != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "--work={0}", settings.Work.MakeAbsolute(_environment).FullPath));
            }

            if (settings.OutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "--out={0}", settings.OutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.ErrorOutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "--err={0}", settings.ErrorOutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.Full)
            {
                builder.Append("--full");
            }

            if (settings.Results != null && settings.NoResults)
            {
                throw new ArgumentException(
                    GetToolName() + ": You can't specify both a results file and set NoResults to true.");
            }

            if (settings.Results != null)
            {
                var results = new StringBuilder(settings.Results.MakeAbsolute(_environment).FullPath);
                if (settings.ResultFormat != null)
                {
                    results.AppendFormat(";format={0}", settings.ResultFormat);
                }
                if (settings.ResultTransform != null)
                {
                    results.AppendFormat(";transform={0}", settings.ResultTransform.MakeAbsolute(_environment).FullPath);
                }
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "--result={0}", results));
            }
            else if (settings.NoResults)
            {
                builder.AppendQuoted("--noresult");
            }

            if (settings.Labels != NUnit3Labels.Off)
            {
                builder.Append("--labels=" + settings.Labels);
            }

            if (settings.TeamCity)
            {
                builder.Append("--teamcity");
            }

            if (settings.NoHeader)
            {
                builder.Append("--noheader");
            }

            if (settings.NoColor)
            {
                builder.Append("--nocolor");
            }

            if (settings.Verbose)
            {
                builder.Append("--verbose");
            }

            if (settings.Configuration != null)
            {
                builder.AppendQuoted("--config=" + settings.Configuration);
            }

            if (settings.Framework != null)
            {
                builder.AppendQuoted("--framework=" + settings.Framework);
            }

            if (settings.X86)
            {
                builder.Append("--x86");
            }

            if (settings.DisposeRunners)
            {
                builder.Append("--dispose-runners");
            }

            if (settings.ShadowCopy)
            {
                builder.Append("--shadowcopy");
            }

            if (settings.Agents.HasValue)
            {
                builder.Append("--agents=" + settings.Agents.Value);
            }

            // don't include the default value
            if (settings.Process != NUnit3ProcessOption.Multiple)
            {
                builder.Append("--process=" + settings.Process);
            }

            if (settings.AppDomainUsage != NUnit3AppDomainUsage.Default)
            {
                builder.Append("--domain=" + settings.AppDomainUsage);
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "NUnit3";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "nunit3-console.exe" };
        }
    }
}
