// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// Contains settings used by <see cref="XUnitRunner" />.
    /// </summary>
    public sealed class XUnitSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether tests should be run as a shadow copy.
        /// Default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tests should be run as a shadow copy; otherwise, <c>false</c>.
        /// </value>
        public bool ShadowCopy { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an XML report should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an XML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool XmlReport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an HTML report should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an HTML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool HtmlReport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not output running test count.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running test count should be outputted; otherwise, <c>false</c>.
        /// </value>
        public bool Silent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitSettings"/> class.
        /// </summary>
        public XUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
