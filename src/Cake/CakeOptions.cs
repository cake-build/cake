// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake
{
    /// <summary>
    /// The options that determines how the application should behave.
    /// </summary>
    public sealed class CakeOptions
    {
        private readonly Dictionary<string, string> _arguments;

        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the build script.
        /// </summary>
        /// <value>The build script.</value>
        public FilePath Script { get; set; }

        /// <summary>
        /// Gets the script arguments.
        /// </summary>
        /// <value>The script arguments.</value>
        public IDictionary<string, string> Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show task descriptions.
        /// </summary>
        /// <value>
        ///   <c>true</c> to show task description; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform a dry run.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a dry run should be performed; otherwise, <c>false</c>.
        /// </value>
        public bool PerformDryRun { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to debug script.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a debug session should be started; otherwise, <c>false</c>.
        /// </value>
        public bool PerformDebug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show help.
        /// </summary>
        /// <value>
        ///   <c>true</c> to show help; otherwise, <c>false</c>.
        /// </value>
        public bool ShowHelp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show version information.
        /// </summary>
        /// <value>
        ///   <c>true</c> to show version information; otherwise, <c>false</c>.
        /// </value>
        public bool ShowVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the Mono compiler or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the mono compiler should be used; otherwise, <c>false</c>.
        /// </value>
        public bool Mono { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the latest roslyn.
        /// </summary>
        /// <value>
        ///   <c>true</c> if latest roslyn should be used; otherwise, <c>false</c>.
        /// </value>
        public bool Experimental { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an error occured during parsing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an error occured during parsing; otherwise, <c>false</c>.
        /// </value>
        public bool HasError { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeOptions"/> class.
        /// </summary>
        public CakeOptions()
        {
            _arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Verbosity = Verbosity.Normal;
            ShowDescription = false;
            ShowHelp = false;
        }
    }
}
