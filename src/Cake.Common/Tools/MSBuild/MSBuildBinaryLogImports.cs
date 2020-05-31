// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// What files to include in the binary log.
    /// </summary>
    public enum MSBuildBinaryLogImports
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
