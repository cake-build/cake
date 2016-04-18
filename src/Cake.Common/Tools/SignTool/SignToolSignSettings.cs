using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.SignTool
{
    /// <summary>
    /// Contains settings used by  <see cref="SignToolSignRunner"/>.
    /// </summary>
    public sealed class SignToolSignSettings : ToolSettings
    {
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

        /// <summary>
        /// Gets or sets the signed content's description.
        /// </summary>
        public string Description { get; set; }
    }
}