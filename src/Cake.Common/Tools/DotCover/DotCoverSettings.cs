// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverTool{TSettings}" />.
    /// </summary>
    public abstract class DotCoverSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value that enables logging and specifies log file name
        /// This represents the <c>/LogFile</c> option.
        /// </summary>
        public FilePath LogFile { get; set; }

        /// <summary>
        /// Gets or sets a value that enables DotCover configuration file.
        /// A configuration file is a reasonable alternative
        /// to specifying all parameters in-line or having them in a batch file.
        /// </summary>
        public FilePath ConfigFile { get; set; }
    }
}
