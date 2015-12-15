using System;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Base class for tool settings.
    /// </summary>
    public class ToolSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the argument customization.
        /// </summary>
        /// <value>The arguments.</value>
        public Func<ProcessArgumentBuilder, ProcessArgumentBuilder> ArgumentCustomization { get; set; }
    }
}