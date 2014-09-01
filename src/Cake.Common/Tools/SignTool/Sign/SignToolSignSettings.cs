using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.SignTool.Sign
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
        /// The tool path.
        /// Defaults to path given by <see cref="SignToolResolver.GetSignToolPath(ICakeEnvironment)"/>
        /// </value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Specify the timestamp server's URL.
        /// </summary>
        public Uri TimeStampUri { get; set; }

        /// <summary>
        /// Path to PFX cerificate
        /// </summary>
        public FilePath CertPath { get; set; }

        /// <summary>
        /// PFX certificate password
        /// </summary>
        public string Password { get; set; }
    }
}
