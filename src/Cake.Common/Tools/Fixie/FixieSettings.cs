// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Fixie
{
    /// <summary>
    /// Contains settings used by <see cref="FixieRunner" />.
    /// </summary>
    public sealed class FixieSettings : ToolSettings
    {
        private readonly IDictionary<string, IList<string>> _options = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets the file to be used to output NUnit style of XML results.
        /// </summary>
        /// <value>
        /// The name of the file to write NUnit style of results.
        /// </value>
        public FilePath NUnitXml { get; set; }

        /// <summary>
        /// Gets or sets the file to be used to output xUnit style of XML results.
        /// </summary>
        /// <value>
        /// The name of the file to write xUnit style of results.
        /// </value>
        public FilePath XUnitXml { get; set; }

        /// <summary>
        /// Gets or sets the the option to force TeamCity-formatted output on or off.
        /// </summary>
        /// <value>
        /// The value of TeamCity-formatted output. Either "on" or "off".
        /// </value>
        public bool? TeamCity { get; set; }

        /// <summary>
        /// Gets the collection of Options as KeyValuePairs.
        /// </summary>
        /// <value>
        /// The collection of keys and values.
        /// </value>
        public IDictionary<string, IList<string>> Options
        {
            get { return _options; }
        }
    }
}
