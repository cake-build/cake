using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.SetProxy
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetSetProxy"/>.
    /// </summary>
    public sealed class NuGetSetProxySettings
    {
        /// <summary>
        /// Gets or sets the path to <c>nuget.exe</c>.
        /// </summary>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file.
        /// If not specified, the file <c>%AppData%\NuGet\NuGet.config</c> is used as the configuration file.
        /// </summary>
        /// <value>The NuGet configuration file.</value>
        public FilePath ConfigFile { get; set; }
    }
}
