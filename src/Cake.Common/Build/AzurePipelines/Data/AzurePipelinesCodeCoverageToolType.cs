// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    ///  Provides the known values for the Code Coverage tool formats.
    /// </summary>
    public enum AzurePipelinesCodeCoverageToolType
    {
        /// <summary>
        /// JaCoCo code coverage format
        /// </summary>
        JaCoCo,

        /// <summary>
        /// Cobertura code coverage format
        /// </summary>
        Cobertura
    }
}
