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
    }
}