// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Specifies framework assemblies to ensure that required references are added to a project.
    /// </summary>
    public class NuSpecFrameworkAssembly
    {
        /// <summary>
        /// Gets or sets the fully qualified assembly name.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the target framework to which this reference applies. If omitted,
        /// indicates that the reference applies to all frameworks.
        /// </summary>
        public string TargetFramework { get; set; }
    }
}
