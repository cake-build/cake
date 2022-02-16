// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.Execute;

namespace Cake.Common.Tools.DotNet.Execute
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreExecutor" />.
    /// </summary>
    public class DotNetExecuteSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets the version of the installed Shared Framework to use to run the application.
        /// </summary>
        public string FrameworkVersion { get; set; }
    }
}
