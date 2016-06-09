// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise provisioning information for the current build.
    /// </summary>
    public class BitriseProvisioningInfo : BitriseInfo
    {
        /// <summary>
        /// Gets the provision URL.
        /// </summary>
        /// <value>
        /// The provision URL.
        /// </value>
        public string ProvisionUrl
        {
            get { return GetEnvironmentString("BITRISE_PROVISION_URL"); }
        }

        /// <summary>
        /// Gets the certificate URL.
        /// </summary>
        /// <value>
        /// The certificate URL.
        /// </value>
        public string CertificateUrl
        {
            get { return GetEnvironmentString("BITRISE_CERTIFICATE_URL"); }
        }

        /// <summary>
        /// Gets the certificate passphrase.
        /// </summary>
        /// <value>
        /// The certificate passphrase.
        /// </value>
        public string CertificatePassphrase
        {
            get { return GetEnvironmentString("BITRISE_CERTIFICATE_PASSPHRASE"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseProvisioningInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseProvisioningInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
