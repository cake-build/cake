using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// Contains settings used by the <see cref="LightRunner"/>.
    /// </summary>
    public sealed class LightSettings
    {
        /// <summary>
        /// Gets or sets the defined WiX variables.
        /// </summary>
        public IDictionary<string, string> Defines { get; set; }

        /// <summary>
        /// Gets or sets the WiX extensions to use.
        /// </summary>
        public IEnumerable<string> Extensions { get; set; }

        /// <summary>
        /// Gets or sets raw command line arguments to pass through to the linker.
        /// </summary>
        public string RawArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the logo information.
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets the path to the output file (i.e. the resulting MSI package).
        /// </summary>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the path to <c>Light.exe</c>.
        /// </summary>
        public FilePath ToolPath { get; set; }
    }
}