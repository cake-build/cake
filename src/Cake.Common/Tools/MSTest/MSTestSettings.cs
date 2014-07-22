using Cake.Core.IO;

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// Contains settings used by <see cref="MSTestRunner"/>.
    /// </summary>
    public sealed class MSTestSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run tests within the MSTest process.
        /// This choice improves test run speed but increases risk to the MSTest.exe process.
        /// Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running without isolation; otherwise, <c>false</c>.
        /// </value>
        public bool NoIsolation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestSettings"/> class.
        /// </summary>
        public MSTestSettings()
        {
            NoIsolation = true;
        }
    }
}