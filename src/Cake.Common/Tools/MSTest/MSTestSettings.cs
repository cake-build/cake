// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
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
        /// Gets or sets a value indicating the test category filter string to pass to
        /// MSTest.exe flag <see href="https://msdn.microsoft.com/en-us/library/ms182489.aspx#category">/testcategory</see>.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the test settings file to pass to MSTest.exe flag <see href="https://msdn.microsoft.com/en-us/library/ms182489.aspx#testsettings">/testsettings</see>.
        /// </summary>
        public FilePath TestSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestSettings"/> class.
        /// </summary>
        public MSTestSettings()
        {
            NoIsolation = true;
        }
    }
}
