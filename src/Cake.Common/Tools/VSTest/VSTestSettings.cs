// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
        /// Gets or sets a value indicating whether to run tests within the vstest.console.exe process.
        /// This makes vstest.console.exe process less likely to be stopped on an error in the tests, but tests might run slower.
        /// Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running in isolation; otherwise, <c>false</c>.
        /// </value>
        public bool InIsolation { get; set; }

        /// <summary>
        /// Gets or sets the target platform architecture to be used for test execution.
        /// </summary>
        public VSTestPlatform PlatformArchitecture { get; set; }

        /// <summary>
        /// Gets or sets the target .NET Framework version to be used for test execution.
        /// </summary>
        public VSTestFrameworkVersion FrameworkVersion { get; set; }

        /// <summary>
        /// Gets or sets the logger to use for test results.
        /// </summary>
        public VSTestLogger Logger { get; set; }
    }
}
