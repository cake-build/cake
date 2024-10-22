// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Reference.Add
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetReferenceAdder" />.
    /// </summary>
    public sealed class DotNetReferenceAddSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets a specific framework.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// For example, to complete authentication. Available since .NET Core 3.0 SDK.
        /// </summary>
        public bool Interactive { get; set; }
    }
}
