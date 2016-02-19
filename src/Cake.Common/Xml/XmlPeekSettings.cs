﻿using System.Collections.Generic;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Contains settings for <see cref="XmlPeekAliases"/>
    /// </summary>
    public sealed class XmlPeekSettings
    {
        /// <summary>
        /// Gets or sets namespaces to include for xpath recognition.
        /// </summary>
        public IDictionary<string, string> Namespaces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to preserve white space.
        /// </summary>
        public bool PreserveWhitespace { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlPeekSettings"/> class.
        /// </summary>
        public XmlPeekSettings()
        {
            PreserveWhitespace = true;
            Namespaces = new Dictionary<string, string>();
        }
    }
}