using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains the common settings used by all commands in <see cref="OctopusDeployRunner"/>.
    /// </summary>
    public abstract class OctopusDeploySettings 
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        public FilePath ToolPath { get; set; }
        
        /// <summary>
        /// Gets or sets the username to use when authenticating with the server
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password to use when authenticating with the server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the text file of default values
        /// </summary>
        public FilePath ConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enable debug logging flag is set
        /// </summary>
        public bool EnableDebugLogging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ignore ssl errors flag is set
        /// </summary>
        public bool IgnoreSslErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enable service messages flag is set
        /// </summary>
        public bool EnableServiceMessages { get; set; }
    }
}