// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.Roundhouse
{
    /// <summary>
    /// Defines the recovery model for SQL Server
    /// </summary>
    public enum RecoveryMode
    {
        /// <summary>
        /// Doesn't change the mode
        /// </summary>
        NoChange,

        /// <summary>
        /// Does not create backup before migration
        /// </summary>
        Simple,

        /// <summary>
        /// Creates log backup before migration
        /// </summary>
        Full
    }
}
