using System.Collections.Generic;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NSIS
{
    /// <summary>
    /// Contains settings used by the <see cref="MakeNSISRunner"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class MakeNSISSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the script compiler defines.
        /// </summary>
        public IDictionary<string, string> Defines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable current directory change to that of the script file.
        /// </summary>
        public bool NoChangeDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable inclusion of the <c>nsisconf.nsh</c> file.
        /// </summary>
        public bool NoConfig { get; set; }
    }
}