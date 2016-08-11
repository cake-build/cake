using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverTool{TSettings}" />.
    /// </summary>
    public abstract class DotCoverSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value that enables logging and specifies log file name
        /// This represents the <c>/LogFile</c> option.
        /// </summary>
        public FilePath LogFile { get; set; }
    }
}
