using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.Cake
{
    /// <summary>
    /// Contains settings used by <see cref="CakeRunner"/>.
    /// </summary>
    public sealed class CakeSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public Verbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets cake additional arguments.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, string> Arguments { get; set; }
    }
}