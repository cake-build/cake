using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains settings used by <see cref="NUnitRunner"/>.
    /// </summary>
    public sealed class NUnitSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tests should be run as a shadow copy.
        /// Default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tests should be run as a shadow copy; otherwise, <c>false</c>.
        /// </value>
        public bool ShadowCopy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitSettings"/> class.
        /// </summary>
        public NUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
