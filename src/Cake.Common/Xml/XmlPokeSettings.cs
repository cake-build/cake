// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Text;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Contains settings for <see cref="XmlPokeAliases"/>
    /// </summary>
    public sealed class XmlPokeSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to preserve white space.
        /// </summary>
        public bool PreserveWhitespace { get; set; }

        /// <summary>
        /// Gets or sets namespaces to include for xpath recognition.
        /// </summary>
        public IDictionary<string, string> Namespaces { get; set; }

        /// <summary>
        /// Gets or sets the type of text encoding to use.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets a value that determines the processing of DTDs.
        /// </summary>
        public XmlDtdProcessing DtdProcessing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlPokeSettings"/> class.
        /// </summary>
        public XmlPokeSettings()
        {
            PreserveWhitespace = true;
            Namespaces = new Dictionary<string, string>();
            Encoding = Encoding.UTF8;
            DtdProcessing = XmlDtdProcessing.Prohibit;
        }
    }
}
