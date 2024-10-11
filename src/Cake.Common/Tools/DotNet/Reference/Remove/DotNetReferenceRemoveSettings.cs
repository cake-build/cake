// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Reference.Remove
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetReferenceRemover" />.
    /// </summary>
    public sealed class DotNetReferenceRemoveSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets a specific framework.
        /// </summary>
        public string Framework { get; set; }
    }
}
