// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.SpecFlow
{
    /// <summary>
    /// Base class for all SpecFlow related tools
    /// </summary>
    /// <typeparam name="TSettings">The settings type</typeparam>
    public abstract class SpecFlowTool<TSettings> : Tool<TSettings>
        where TSettings : SpecFlowSettings
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecFlowTool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected SpecFlowTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "SpecFlow";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "specflow.exe", "SpecFlow.exe" };
        }

        /// <summary>
        /// Appends the SpecFlowSettings arguments to builder.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="builder">The argument builder.</param>
        protected void AppendArguments(
            SpecFlowSettings settings,
            ProcessArgumentBuilder builder)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            // Set the out file.
            if (settings.Out != null)
            {
                var outFile = settings.Out.MakeAbsolute(_environment);
                builder.AppendSwitch("/out", ":", outFile.FullPath.Quote());
            }

            // Set the xslt file.
            if (settings.XsltFile != null)
            {
                var xsltFile = settings.XsltFile.MakeAbsolute(_environment);
                builder.AppendSwitch("/xsltFile", ":", xsltFile.FullPath.Quote());
            }
        }
    }
}
