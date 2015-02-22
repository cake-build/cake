using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.SignTool
{
    /// <summary>
    /// Contains settings used by  <see cref="SignToolSignRunner"/>.
    /// </summary>
    public sealed class SignToolSignSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>
        /// The tool path. Defaults to path given by <see cref="SignToolResolver.GetSignToolPath(ICakeEnvironment)"/>
        /// </value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the timestamp server's URL.
        /// </summary>
        public Uri TimeStampUri { get; set; }

        /// <summary>
        /// Gets or sets the <c>PFX</c> certificate path.
        /// </summary>
        public FilePath CertPath { get; set; }

        /// <summary>
        /// Gets or sets the <c>PFX</c> certificate password.
        /// </summary>
        public string Password { get; set; }
    }
}
