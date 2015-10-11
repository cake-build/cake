using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// Contains settings used by <see cref="CandleRunner"/>.
    /// </summary>
    public sealed class CandleSettings
    {
        /// <summary>
        /// Gets or sets a value indicating which architecture to build the MSI package for.
        /// </summary>
        public Architecture? Architecture { get; set; }

        /// <summary>
        /// Gets or sets the pre processor defines.
        /// </summary>
        public IDictionary<string, string> Defines { get; set; }

        /// <summary>
        /// Gets or sets the WiX extensions to use.
        /// </summary>
        public IEnumerable<string> Extensions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether FIPS compliant algorithms should be used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if FIPS compliant algorithms should be used, otherwise <c>false</c>.
        /// </value>
        public bool FIPS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the logo information.
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets the output directory for the object files.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show pedantic messages.
        /// </summary>
        public bool Pedantic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show source trace for errors, warnings and verbose messages.
        /// </summary>
        public bool ShowSourceTrace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show verbose output.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets the path to <c>Candle.exe</c>.
        /// </summary>
        public FilePath ToolPath { get; set; }
    }
}