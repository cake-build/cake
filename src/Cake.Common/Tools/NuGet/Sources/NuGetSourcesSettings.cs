using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Sources
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetSources"/>.
    /// </summary>
    public sealed class NuGetSourcesSettings
    {
        /// <summary>
        /// Gets or sets the (optional) user name.
        /// </summary>
        /// <value>Optional user name to be used when connecting to an authenticated source.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the (optional) password.
        /// </summary>
        /// <value>Optional password to be used when connecting to an authenticated source.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this source contains sensitive data, i.e. authentication token in url.
        /// </summary>
        /// <value>
        /// <c>true</c> if this source contains sensitive data; otherwise, <c>false</c>.
        /// </value>
        public bool IsSensitiveSource { get; set; }

        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }
    }
}