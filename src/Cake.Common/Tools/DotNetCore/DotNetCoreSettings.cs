using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// Contains common settings used by <see cref="DotNetCoreTool{TSettings}" />.
    /// </summary>
    public abstract class DotNetCoreSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to not enable verbose output.
        /// </summary>
        public bool Verbose { get; set; }
    }
}
