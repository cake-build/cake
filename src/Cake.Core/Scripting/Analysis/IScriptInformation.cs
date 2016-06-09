// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents information about a script.
    /// </summary>
    public interface IScriptInformation
    {
        /// <summary>
        /// Gets the script path.
        /// </summary>
        /// <value>The script path.</value>
        FilePath Path { get; }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        /// <value>The includes.</value>
        IList<IScriptInformation> Includes { get; }

        /// <summary>
        /// Gets the referenced script assemblies.
        /// </summary>
        /// <value>The referenced script assemblies.</value>
        IList<string> References { get; }

        /// <summary>
        /// Gets all namespaces referenced by the script.
        /// </summary>
        /// <value>The namespaces referenced by the script.</value>
        IList<string> Namespaces { get; }

        /// <summary>
        /// Gets all using aliases defined by the scripts.
        /// </summary>
        /// <value>The using aliases defined by the script.</value>
        IList<string> UsingAliases { get; }

        /// <summary>
        /// Gets the tools.
        /// </summary>
        /// <value>The tools.</value>
        IList<PackageReference> Tools { get; }

        /// <summary>
        /// Gets the addins.
        /// </summary>
        /// <value>The addins.</value>
        IList<PackageReference> Addins { get; }
    }
}
