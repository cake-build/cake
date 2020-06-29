// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tooling;

namespace Cake.Common.Tools.VSWhere
{
    /// <summary>
    /// Base class for all settings for VSWhere tools.
    /// </summary>
    public abstract class VSWhereSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the workload(s) or component(s) required when finding instances.
        /// </summary>
        public string Requires { get; set; }

        /// <summary>
        /// Gets or sets version range for instances to find. Example: ["15.0","16.0"] will find versions 15.*.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the property to return. Defaults to "value" format.
        /// </summary>
        public string ReturnProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether VSWhere should include prerelease installations.
        /// </summary>
        public bool IncludePrerelease { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSWhereSettings"/> class.
        /// </summary>
        protected VSWhereSettings()
        {
            ReturnProperty = "installationPath";
        }
    }
}
