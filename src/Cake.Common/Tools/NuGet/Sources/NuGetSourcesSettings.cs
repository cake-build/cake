using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Sources
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetSources"/>.
    /// </summary>
    public sealed class NuGetSourcesSettings
    {
        private static readonly NuGetSourcesSettings _default =new NuGetSourcesSettings();
        /// <summary>
        /// Default settings
        /// </summary>
        public static NuGetSourcesSettings Default {get { return _default; }}

        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        /// <value>Optional UserName to be used when connecting to an authenticated source.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        /// <value>Optional Password to be used when connecting to an authenticated source.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the IsSensitiveSource
        /// </summary>
        /// <value>Flag for if source contains sensitive data i.e. auth token in url</value>
        public bool IsSensitiveSource { get; set; }
        

        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }
    }
}
