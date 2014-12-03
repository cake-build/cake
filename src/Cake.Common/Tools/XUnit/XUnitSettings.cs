using Cake.Core.IO;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// Contains settings used by <see cref="XUnitRunner"/>.
    /// </summary>
    public class XUnitSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether tests should be run as a shadow copy.
        /// Default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tests should be run as a shadow copy; otherwise, <c>false</c>.
        /// </value>
        public bool ShadowCopy { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an XML report should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an XML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool XmlReport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an HTML report should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an HTML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool HtmlReport { get; set; }

        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or set whether or not output running test count.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitSettings"/> class.
        /// </summary>
        public XUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
