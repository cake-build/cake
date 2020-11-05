// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Test
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreTester" />.
    /// </summary>
    public sealed class DotNetCoreTestSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets the settings file to use when running tests.
        /// </summary>
        public FilePath Settings { get; set; }

        /// <summary>
        /// Gets or sets the filter expression to filter out tests in the current project.
        /// </summary>
        /// <remarks>
        /// For more information on filtering support, see https://aka.ms/vstest-filtering.
        /// </remarks>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the path to use for the custom test adapter in the test run.
        /// </summary>
        public DirectoryPath TestAdapterPath { get; set; }

        /// <summary>
        /// Gets or sets a logger for test results.
        /// </summary>
        [Obsolete("Please use Loggers instead.")]
        public string Logger { get; set; }

        /// <summary>
        /// Gets or sets the loggers for test results.
        /// </summary>
        public ICollection<string> Loggers { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the configuration under which to build.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the data collectors for the test run.
        /// </summary>
        public ICollection<string> Collectors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not build the project before testing.
        /// </summary>
        public bool NoBuild { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not do implicit NuGet package restore.
        /// This makes build faster, but requires restore to be done before build is executed.
        /// </summary>
        /// <remarks>
        /// Requires .NET Core 2.x or newer.
        /// </remarks>
        public bool NoRestore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run tests without displaying the Microsoft TestPlatform banner.
        /// </summary>
        /// <remarks>
        /// Available since .NET Core 3.0 SDK.
        /// </remarks>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets a file to write diagnostic messages to.
        /// </summary>
        public FilePath DiagnosticFile { get; set; }

        /// <summary>
        /// Gets or sets the results directory. This setting is only available from 2.0.0 upward.
        /// </summary>
        public DirectoryPath ResultsDirectory { get; set; }

        /// <summary>
        /// Gets or sets the file path to write VSTest reports to.
        /// </summary>
        public FilePath VSTestReportPath { get; set; }

        /// <summary>
        /// Gets or sets the target runtime to test for. This setting is only available from .NET Core 3.x upward.
        /// </summary>
        public string Runtime { get; set; }
    }
}
