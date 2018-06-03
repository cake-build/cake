// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core
{
    /// <summary>
    /// Represents a Cake report entry category.
    /// </summary>
    public enum CakeReportEntryCategory
    {
        /// <summary>
        /// Represents a normal task.
        /// </summary>
        Task,

        /// <summary>
        /// Represent a setup task.
        /// </summary>
        Setup,

        /// <summary>
        /// Represent a teardown task.
        /// </summary>
        Teardown
    }
}