// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Gets or sets the timestamp server's URL. Timestamp will only be added if <c>TimeStampUri</c> is set.
        /// </summary>
        public Uri TimeStampUri { get; set; }

        /// <summary>
        /// Gets or sets the thumbprint for locating a certificate in the store.
        /// </summary>
        public string CertThumbprint { get; set; }

        /// <summary>
        /// Gets or sets the name of the subject of the signing certificate. This value can be a substring of the entire subject name.
        /// </summary>
        public string CertSubjectName { get; set; }

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

        /// <summary>
        /// Gets or sets the signed content's expanded description URL.
        /// </summary>
        public Uri DescriptionUri { get; set; }

        /// <summary>
        /// Gets or sets the file digest algorithm.
        /// </summary>
        public SignToolDigestAlgorithm DigestAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the timestamp digest algorithm.
        /// </summary>
        public SignToolDigestAlgorithm TimeStampDigestAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the signature should be appended.
        /// </summary>
        public bool AppendSignature { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a machine store, instead of a user store, is used.
        /// </summary>
        public bool UseMachineStore { get; set; }

        /// <summary>
        /// Gets or sets the path to an additional certificate that is to be added to the signature block.
        /// </summary>
        public FilePath AdditionalCertPath { get; set; }

        /// <summary>
        /// Gets or sets the store to open when searching for the certificate.
        /// </summary>
        public string StoreName { get; set; }
    }
}