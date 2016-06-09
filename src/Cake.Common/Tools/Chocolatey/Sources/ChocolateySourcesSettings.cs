// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Sources
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateySources"/>.
    /// </summary>
    public sealed class ChocolateySourcesSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to run in debug mode.
        /// </summary>
        /// <value>The debug flag</value>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in verbose mode.
        /// </summary>
        /// <value>The verbose flag.</value>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in forced mode.
        /// </summary>
        /// <value>The force flag</value>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in noop mode.
        /// </summary>
        /// <value>The noop flag.</value>
        public bool Noop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in limited output mode.
        /// </summary>
        /// <value>The limit output flag</value>
        public bool LimitOutput { get; set; }

        /// <summary>
        /// Gets or sets the execution timeout value.
        /// </summary>
        /// <value>The execution timeout</value>
        /// <remarks>Default is 2700 seconds</remarks>
        public int ExecutionTimeout { get; set; }

        /// <summary>
        /// Gets or sets the location of the download cache.
        /// </summary>
        /// <value>The download cache location</value>
        public string CacheLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in allow unofficial mode.
        /// </summary>
        /// <value>The allow unofficial flag</value>
        public bool AllowUnofficial { get; set; }

        /// <summary>
        /// Gets or sets the (optional) user name.
        /// </summary>
        /// <value>Optional user name to be used when connecting to an authenticated source.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the (optional) password.
        /// </summary>
        /// <value>Optional password to be used when connecting to an authenticated source.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the (optional) priority.
        /// </summary>
        /// <value>Optional priority to be used when creating source.</value>
        public int Priority { get; set; }
    }
}
