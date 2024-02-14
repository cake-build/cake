// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// The problem information.
    /// </summary>
    public sealed class DotNetPackageListProblemItem
    {
        /// <summary>
        /// Gets the problem level.
        /// </summary>
        [JsonInclude]
        public DotNetPackageListProblemType? Level { get; private set; }

        /// <summary>
        /// Gets the problem text.
        /// </summary>
        [JsonInclude]
        public string Text { get; private set; }

        /// <summary>
        /// Gets the project path.
        /// </summary>
        [JsonInclude]
        public string Project { get; private set; }
    }
}
