// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// What files to include in the binary log.
    /// </summary>
    public enum MSBuildBinaryLoggerImports
    {
        /// <summary>Don't specify imports</summary>
        Unspecified = 0,

        /// <summary>Do not collect project and imports files</summary>
        None = 2,

        /// <summary>Embed in the binlog file</summary>
        Embed = 3,

        /// <summary>Produce a separate .ProjectImports.zip</summary>
        ZipFile = 4
    }
}
