// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// InspectCode runner
    /// </summary>
    public sealed class InspectCodeRunner : Tool<InspectCodeSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectCodeRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="log">The logger.</param>
        public InspectCodeRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        /// <summary>
        /// Analyses the specified solution, using the specified settings.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath solution, InspectCodeSettings settings)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(settings, solution));

            if (settings.OutputFile != null)
            {
                AnalyseResultsFile(settings.OutputFile, settings.ThrowExceptionOnFindingViolations);
            }
        }

        /// <summary>
        /// Runs ReSharper's InspectCode using the provided config file.
        /// </summary>
        /// <param name="configFile">The config file.</param>
        public void RunFromConfig(FilePath configFile)
        {
            if (configFile == null)
            {
                throw new ArgumentNullException("configFile");
            }

            Run(new InspectCodeSettings(), GetConfigArgument(configFile));
        }

        // ReSharper disable once UnusedParameter.Local
        private void AnalyseResultsFile(FilePath resultsFilePath, bool throwOnViolations)
        {
            var anyFailures = false;
            var resultsFile = _fileSystem.GetFile(resultsFilePath);

            using (var stream = resultsFile.OpenRead())
            {
                var xmlDoc = XDocument.Load(stream);
                var violations = xmlDoc.Descendants("IssueType").Where(i => i.Attribute("Severity") != null && i.Attribute("Severity").Value == "ERROR");

                foreach (var violation in violations)
                {
                    _log.Warning("Code Inspection Error(s) Located. Description: {0}", violation.Attribute("Description").Value);

                    var tempViolation = violation; // Copy to temporary variable to avoid side effects.
                    var issueLookups = xmlDoc.Descendants("Issue").Where(i => i.Attribute("TypeId") != null && i.Attribute("TypeId").Value == tempViolation.Attribute("Id").Value);

                    foreach (var issueLookup in issueLookups)
                    {
                        var file = issueLookup.Attribute("File") == null ? string.Empty : issueLookup.Attribute("File").Value;
                        var line = issueLookup.Attribute("Line") == null ? string.Empty : issueLookup.Attribute("Line").Value;
                        var message = issueLookup.Attribute("Message") == null ? string.Empty : issueLookup.Attribute("Message").Value;

                        _log.Warning("File Name: {0} Line Number: {1} Message: {2}", file, line, message);
                    }

                    anyFailures = true;
                }
            }

            if (anyFailures && throwOnViolations)
            {
                throw new CakeException("Code Inspection Violations found in code base.");
            }
        }

        private ProcessArgumentBuilder GetConfigArgument(FilePath configFile)
        {
            var builder = new ProcessArgumentBuilder();
            builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/config={0}",
                configFile.MakeAbsolute(_environment).FullPath));

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(InspectCodeSettings settings, FilePath solution)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.OutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/output:{0}", settings.OutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.SolutionWideAnalysis && settings.NoSolutionWideAnalysis)
            {
                throw new ArgumentException(
                    GetToolName() + ": You can't set both SolutionWideAnalysis and NoSolutionWideAnalysis to true");
            }

            if (settings.SolutionWideAnalysis)
            {
                builder.Append("/swea");
            }

            if (settings.NoSolutionWideAnalysis)
            {
                builder.Append("/no-swea");
            }

            if (settings.ProjectFilter != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/project={0}", settings.ProjectFilter));
            }

            if (settings.MsBuildProperties != null)
            {
                foreach (var property in settings.MsBuildProperties)
                {
                    builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/properties:{0}={1}", property.Key, property.Value));
                }
            }

            if (settings.Extensions != null && settings.Extensions.Any())
            {
                builder.AppendQuoted("/extensions=" + string.Join(";", settings.Extensions));
            }

            if (settings.CachesHome != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/caches-home={0}", settings.CachesHome.MakeAbsolute(_environment).FullPath));
            }

            if (settings.Debug)
            {
                builder.Append("/debug");
            }

            if (settings.NoBuildinSettings)
            {
                builder.Append("/no-buildin-settings");
            }

            if (settings.DisabledSettingsLayers != null && settings.DisabledSettingsLayers.Any())
            {
                builder.AppendQuoted("/dsl=" + string.Join(";", settings.DisabledSettingsLayers));
            }

            if (settings.Profile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/profile={0}", settings.Profile.MakeAbsolute(_environment).FullPath));
            }

            builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "InspectCode";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "inspectcode.exe" };
        }
    }
}
