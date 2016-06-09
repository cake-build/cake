// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// Contains settings used by <see cref="XUnit2Runner"/>.
    /// </summary>
    public sealed class XUnit2Settings : ToolSettings
    {
        private int? _maxThreads;

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
        /// Gets or sets a value indicating whether an xUnit.net v1 style XML report should be generated.
        /// </summary>
        public bool XmlReportV1 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an HTML report should be generated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an HTML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool HtmlReport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not use app domains to run test code.
        /// </summary>
        /// <value>
        ///   <c>true</c> to not use app domains to run test code; otherwise, <c>false</c>.
        /// </value>
        public bool NoAppDomain { get; set; }

        /// <summary>
        /// Gets or sets the parallelism option.
        /// Corresponds to the -parallel command line switch.
        /// </summary>
        /// <value>
        /// The parallelism option.
        /// </value>
        public ParallelismOption Parallelism { get; set; }

        /// <summary>
        /// Gets or sets the maximum thread count for collection parallelization.
        /// </summary>
        /// <value>
        ///   <c>null</c> (default);
        ///   <c>0</c>: run with unbounded thread count;
        ///   <c>&gt;0</c>: limit task thread pool size to value;
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException" accessor="set">value &lt; 0</exception>
        public int? MaxThreads
        {
            get
            {
                return _maxThreads;
            }

            set
            {
                if (value.HasValue && value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Value may not be negative.");
                }
                _maxThreads = value;
            }
        }

        /// <summary>
        /// Gets the traits to include.
        /// </summary>
        /// <remarks>
        /// Only run tests with matching name/value traits.
        /// If more than one is specified, it acts as an OR operation.
        /// </remarks>
        /// <value>
        /// The traits to include.
        /// </value>
        public IDictionary<string, IList<string>> TraitsToInclude { get; private set; }

        /// <summary>
        /// Gets the traits to exclude.
        /// </summary>
        /// <remarks>
        /// Do not run tests with matching name/value traits.
        /// If more than one is specified, it acts as an AND operation.
        /// </remarks>
        /// <value>
        /// The traits to exclude.
        /// </value>
        public IDictionary<string, IList<string>> TraitsToExclude { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnit2Settings"/> class.
        /// </summary>
        public XUnit2Settings()
        {
            TraitsToInclude = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
            TraitsToExclude = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
            ShadowCopy = true;
        }
    }
}
