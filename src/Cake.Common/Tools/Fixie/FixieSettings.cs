namespace Cake.Common.Tools.Fixie
{
    using Core.IO;

    /// <summary>
    /// Contains settings used by <see cref="FixieRunner" />.
    /// </summary>
    public sealed class FixieSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>
        /// The tool path. Defaults to <c>./tools/**/Fixie.Console.exe</c>.
        /// </value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the file to be used to output NUnit style of XML results.
        /// </summary>
        /// <value>
        /// The name of the file to write NUnit style of results.
        /// </value>
        public FilePath NUnitXml { get; set; }

        /// <summary>
        /// Gets or sets the file to be used to output xUnit style of XML results.
        /// </summary>
        /// <value>
        /// The name of the file to write xUnit style of results.
        /// </value>
        public FilePath XUnitXml { get; set; }
    }
}