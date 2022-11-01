// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateySettings"/>.
    /// </summary>
    public class ChocolateySettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to run in debug mode.
        /// </summary>
        /// <value>The debug flag.</value>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in verbose mode.
        /// </summary>
        /// <value>The verbose flag.</value>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in trace mode.
        /// </summary>
        /// <value>The trace flag.</value>
        public bool Trace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in no color mode.
        /// </summary>
        /// <value>The no-color flag.</value>
        public bool NoColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to accept license for package.
        /// </summary>
        /// <value>The accept-license flag.</value>
        public bool AcceptLicense { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in forced mode.
        /// </summary>
        /// <value>The force flag.</value>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in noop mode.
        /// </summary>
        /// <value>The noop flag.</value>
        public bool Noop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in limited output mode.
        /// </summary>
        /// <value>The limit-output flag.</value>
        public bool LimitOutput { get; set; }

        /// <summary>
        /// Gets or sets the execution timeout value.
        /// </summary>
        /// <value>The execution timeout.</value>
        /// <remarks>Default is 2700 seconds.</remarks>
        public int ExecutionTimeout { get; set; }

        /// <summary>
        /// Gets or sets the location of the download cache.
        /// </summary>
        /// <value>The download cache location.</value>
        public string CacheLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in allow unofficial mode.
        /// </summary>
        /// <value>The allow-unofficial flag.</value>
        public bool AllowUnofficial { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to faile when error output is detected.
        /// </summary>
        /// <value>The fail-on-error-output flag.</value>
        public bool FailOnErrorOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run using system installed version of PowerShell.
        /// </summary>
        /// <value>The use-system-powershell flag.</value>
        public bool UseSystemPowerShell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run while not showing download progress.
        /// </summary>
        /// <value>The no-progress flag.</value>
        public bool NoProgress { get; set; }

        /// <summary>
        /// Gets or sets the explicit proxy location.
        /// </summary>
        /// <value>The proxy location.</value>
        public string Proxy { get; set; }

        /// <summary>
        /// Gets or sets the explicit proxy user.
        /// </summary>
        /// <value>The proxy user.</value>
        public string ProxyUser { get; set; }

        /// <summary>
        /// Gets or sets the explicit proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// Gets or sets the comma separated list of regex location to bypass on proxy.
        /// </summary>
        /// <value>The bypass proxy list.</value>
        public string ProxyByPassList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to bypass proxy for local connections.
        /// </summary>
        /// <value>The proxy-bypass-on-local flag.</value>
        public bool ProxyBypassOnLocal { get; set; }

        /// <summary>
        /// Gets or sets the path to the file where all log entries will be sent.
        /// </summary>
        public FilePath LogFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip all compatibility checks.
        /// </summary>
        /// <value>The skip-compatibility-checks flag.</value>
        public bool SkipCompatibilityChecks { get; set; }
    }
}
