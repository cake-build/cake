﻿using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// Contains settings used by <see cref="MSTestRunner"/>.
    /// </summary>
    public sealed class MSTestSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to run tests within the MSTest process.
        /// This choice improves test run speed but increases risk to the MSTest.exe process.
        /// Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if running without isolation; otherwise, <c>false</c>.
        /// </value>
        public bool NoIsolation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestSettings"/> class.
        /// </summary>
        public MSTestSettings()
        {
            NoIsolation = true;
        }
    }
}