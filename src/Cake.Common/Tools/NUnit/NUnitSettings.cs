﻿using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains settings used by <see cref="NUnitRunner" />.
    /// </summary>
    public sealed class NUnitSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>
        /// The tool path. Defaults to <c>./tools/**/nunit-console.exe</c>.
        /// </value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML result file.
        /// </summary>
        /// <value>
        /// The name of the XML result file. Defaults to <c>TestResult.xml</c>.
        /// </value>
        public FilePath ResultsFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate the XML result file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the XML result file should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool NoResults { get; set; }

        /// <summary>
        /// Gets or sets the version of the runtime to be used when executing tests.
        /// </summary>
        /// <value>
        /// The version of the runtime to be used when executing tests.
        /// </value>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the categories to include in a run.
        /// </summary>
        /// <value>The categories to include in a run.</value>
        public string Include { get; set; }

        /// <summary>
        /// Gets or sets the categories to exclude from a run.
        /// </summary>
        /// <value>
        /// The categories to exclude from a run.
        /// </value>
        public string Exclude { get; set; }

        /// <summary>
        /// Gets or sets the default timeout to be used for test cases in this run.
        /// If any test exceeds the timeout value, it is cancelled and reported as an error.
        /// </summary>
        /// <value>The default timeout to be used for test cases in this run.</value>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tests should be run as a shadow copy.
        /// Default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tests should be run as a shadow copy; otherwise, <c>false</c>.
        /// </value>
        public bool ShadowCopy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the main thread should be used for running tests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the main thread should be used for running tests; otherwise, <c>false</c>.
        /// </value>
        public bool NoThread { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show copyright information at the start of the program.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to show copyright information at the start of the program; otherwise, <c>false</c>.
        /// </value>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether execution of the test run should terminate 
        /// immediately on the first test failure or error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if execution of the test run should terminate immediately on the first test failure or error; otherwise, <c>false</c>.
        /// </value>
        public bool StopOnError { get; set; }

        /// <summary>
        /// Gets or sets the amount of information that NUnit should write to its internal trace log.
        /// </summary>
        /// <value>The amount of information that NUnit should write to its internal trace log.</value>
        public string Trace { get; set; }

        /// <summary>
        /// Gets or sets the location that NUnit should write test output.
        /// </summary>
        /// <value>The location that NUnit should write test output.</value>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the location that NUnit should write test error output.
        /// </summary>
        /// <value>The location that NUnit should write test error output.</value>
        public FilePath ErrorOutputFile { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitSettings" /> class.
        /// </summary>
        public NUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
