// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.VSTest;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.VSTest
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreVSTester" />.
    /// </summary>
    public sealed class DotNetCoreVSTestSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets the settings file to use when running tests.
        /// </summary>
        public FilePath Settings { get; set; }

        /// <summary>
        /// Gets or sets the a list tests to run.
        /// </summary>
        public ICollection<string> TestsToRun { get; set; }

        /// <summary>
        /// Gets or sets the path to use for the custom test adapter in the test run.
        /// </summary>
        public DirectoryPath TestAdapterPath { get; set; }

        /// <summary>
        /// Gets or sets the target platform architecture to be used for test execution.
        /// </summary>
        public VSTestPlatform Platform { get; set; }

        /// <summary>
        /// Gets or sets specific .Net Framework version to be used for test execution.
        /// </summary>
        /// <remarks>
        /// Valid values are ".NETFramework,Version=v4.6", ".NETCoreApp,Version=v1.0" etc.
        /// Other supported values are Framework35, Framework40, Framework45 and FrameworkCore10.
        /// </remarks>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tests should be executed in parallel.
        /// </summary>
        /// <remarks>
        /// By default up to all available cores on the machine may be used. The number of cores to use may be configured using a settings file.
        /// </remarks>
        public bool Parallel { get; set; }

        /// <summary>
        /// Gets or sets the filter expression to run test that match.
        /// </summary>
        /// <remarks>
        /// For more information on filtering support, see https://aka.ms/vstest-filtering
        /// </remarks>
        public string TestCaseFilter { get; set; }

        /// <summary>
        /// Gets or sets a logger for test results.
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// Gets or sets the Process Id of the Parent Process responsible for launching current process.
        /// </summary>
        public string ParentProcessId { get; set; }

        /// <summary>
        /// Gets or sets the Port for socket connection and receiving the event messages.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Gets or sets a file to write diagnostic messages to.
        /// </summary>
        public FilePath DiagnosticFile { get; set; }

        /// <summary>
        /// Gets or sets a list of extra arguments that should be passed to adapter.
        /// </summary>
        public IDictionary<string, string> Arguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreVSTestSettings"/> class.
        /// </summary>
        public DotNetCoreVSTestSettings()
        {
            TestsToRun = new List<string>();
            Arguments = new Dictionary<string, string>();
        }
    }
}
