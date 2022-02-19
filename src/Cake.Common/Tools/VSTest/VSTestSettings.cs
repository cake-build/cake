// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Contains settings used by <see cref="VSTestRunner"/>.
    /// </summary>
    public sealed class VSTestSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the settings filename to be used to control additional settings such as data collectors.
        /// </summary>
        public FilePath SettingsFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tests are executed in parallel. By default up to all available cores on the machine may be used. The number of cores to use may be configured using a settings file.
        /// </summary>
        public bool Parallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable data diagnostic adapter 'CodeCoverage' in the test run. Default settings are used if not specified using settings file.
        /// </summary>
        public bool EnableCodeCoverage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run tests within the vstest.console.exe process.
        /// This makes vstest.console.exe process less likely to be stopped on an error in the tests, but tests might run slower.
        /// Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running in isolation; otherwise, <c>false</c>.
        /// </value>
        public bool InIsolation { get; set; }

        /// <summary>
        /// Gets or sets a value overriding whether VSTest will use or skip the VSIX extensions installed (if any) in the test run.
        /// </summary>
        public bool? UseVsixExtensions { get; set; }

        /// <summary>
        /// Gets or sets a value that makes VSTest use custom test adapters from a given path (if any) in the test run.
        /// </summary>
        public DirectoryPath TestAdapterPath { get; set; }

        /// <summary>
        /// Gets or sets the target platform architecture to be used for test execution.
        /// </summary>
        public VSTestPlatform PlatformArchitecture { get; set; }

        /// <summary>
        /// Gets or sets the target .NET Framework version to be used for test execution.
        /// </summary>
        public VSTestFrameworkVersion FrameworkVersion { get; set; }

        /// <summary>
        /// Gets or sets an expression to run only tests that match, of the format &lt;property&gt;Operator&lt;value&gt;[|&amp;&lt;Expression&gt;]
        ///     where Operator is one of =, != or ~  (Operator ~ has 'contains'
        ///     semantics and is applicable for string properties like DisplayName).
        ///     Parenthesis () can be used to group sub-expressions.
        /// Examples: Priority=1
        ///           (FullyQualifiedName~Nightly|Name=MyTestMethod).
        /// </summary>
        public string TestCaseFilter { get; set; }

        /// <summary>
        /// Gets or sets a path which makes VSTest write diagnosis trace logs to specified file.
        /// </summary>
        public FilePath Diag { get; set; }

        /// <summary>
        /// Gets or sets the result directory.
        /// Test results directory will be created in specified path if not exists.
        /// VSTest.Console.exe flag <see href="https://docs.microsoft.com/en-us/visualstudio/test/vstest-console-options#ResultDirectory">/ResultsDirectory</see>.
        /// </summary>
        public DirectoryPath ResultsDirectory { get; set; }

        /// <summary>
        /// Gets or sets the name of your logger. Possible values:
        /// - A blank string (or null): no logger
        /// - "trx": Visual Studio's built-in logger
        /// - "AppVeyor": AppVeyor's custom logger which is available only when building your solution on the AppVeyor platform
        /// - any custom value: the name of your custom logger.
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tools from a preview edition of Visual Studio should be used.
        /// <para>
        /// If set to <c>true</c>, VSTest from a Preview edition
        /// (e.g. Visual Studio 2022 Preview) will be considered to be used.
        /// </para>
        /// </summary>
        public bool AllowPreviewVersion { get; set; } = false;
    }
}