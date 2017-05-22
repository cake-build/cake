// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains settings used by <see cref="NUnit3Runner" />.
    /// </summary>
    public sealed class NUnit3Settings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the list of tests to run or explore.
        /// </summary>
        /// <value>
        /// A comma-separated list of test names.
        /// </value>
        public string Test { get; set; }

        /// <summary>
        /// Gets or sets a file containing the tests to run.
        /// </summary>
        /// <value>
        /// File path containing a list of tests to run, one per line.
        /// </value>
        public FilePath TestList { get; set; }

        /// <summary>
        /// Gets or sets the test selection expression indicating what tests will be run.
        /// </summary>
        /// <value>
        /// The --where option is intended to extend or replace the earlier
        /// --test, --include and --exclude options by use of a selection expression
        /// describing exactly which tests to use. Examples of usage are:
        ///    --where:cat==Data
        ///    --where "method =~ /DataTest*/ &amp;&amp; cat = Slow"
        /// See <a href="https://github.com/nunit/docs/wiki/Test-Selection-Language">https://github.com/nunit/docs/wiki/Test-Selection-Language</a>.
        /// </value>
        public string Where { get; set; }

        /// <summary>
        /// Gets or sets the default timeout to be used for test cases in this run.
        /// If any test exceeds the timeout value, it is cancelled and reported as an error.
        /// </summary>
        /// <value>
        /// The timeout in milliseconds.
        /// </value>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the random seed used to generate test cases.
        /// </summary>
        /// <value>
        /// The random seed.
        /// </value>
        public int? Seed { get; set; }

        /// <summary>
        /// Gets or sets the number of worker threads to be used
        /// in running tests.If not specified, defaults to
        /// 2 or the number of processors, whichever is greater.
        /// </summary>
        /// <value>
        /// The number of worker threads.
        /// </value>
        public int? Workers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether execution of the test run should terminate
        /// immediately on the first test failure or error.
        /// </summary>
        /// <value>
        /// <c>true</c> if execution of the test run should terminate immediately on the first test failure or error;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool StopOnError { get; set; }

        /// <summary>
        /// Gets or sets the directory to use for output files. If
        /// not specified, defaults to the current directory.
        /// </summary>
        /// <value>
        /// PATH of the directory.
        /// </value>
        public DirectoryPath Work { get; set; }

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
        /// Gets or sets a value indicating whether to print full report of all test results.
        /// </summary>
        /// <value>
        /// <c>true</c> if a full report of test results should be printed;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool Full { get; set; }

        /// <summary>
        /// Gets or sets the name of the XML result file.
        /// </summary>
        /// <value>
        /// The name of the XML result file. Defaults to <c>TestResult.xml</c>.
        /// </value>
        public FilePath Results { get; set; }

        /// <summary>
        /// Gets or sets the format that the results should be in. Results must be set to
        /// have any effect. Specify nunit2 to output the results in NUnit 2 xml format.
        /// nunit3 may be specified for NUnit 3 format, however this is the default. Additional
        /// formats may be supported in the future, check the NUnit documentation.
        /// </summary>
        /// <value>
        /// The format of the result file. Defaults to <c>nunit3</c>.
        /// </value>
        public string ResultFormat { get; set; }

        /// <summary>
        /// Gets or sets the file name of an XSL transform that will be applied to the results.
        /// </summary>
        /// <value>
        /// The name of an XSLT file that will be applied to the results.
        /// </value>
        public FilePath ResultTransform { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate the XML result file.
        /// </summary>
        /// <value>
        /// <c>true</c> if the XML result file should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool NoResults { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether to write test case names to the output.
        /// </summary>
        /// <value>
        /// <c>On</c> to write labels for tests that are run or <c>All</c> to write labels
        /// for all tests.
        /// </value>
        public NUnit3Labels Labels { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to turn on TeamCity service messages.
        /// </summary>
        /// <value>
        /// <c>true</c> to turn on TeamCity service messages; otherwise, <c>false</c>.
        /// </value>
        public bool TeamCity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show copyright information at the start of the program.
        /// </summary>
        /// <value>
        /// <c>true</c> if to show copyright information at the start of the program; otherwise, <c>false</c>.
        /// </value>
        public bool NoHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the output in color.
        /// </summary>
        /// <value>
        /// <c>true</c> disable color output; otherwise, <c>false</c>.
        /// </value>
        public bool NoColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show additional information as the tests run.
        /// </summary>
        /// <value>
        /// <c>true</c> shows additional information as the tests run; otherwise, <c>false</c>.
        /// </value>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets the name of a project configuration to load (e.g.:Debug).
        /// This selects the configuration within the NUnit project file.
        /// </summary>
        /// <value>
        /// The name of the configuration to load.
        /// </value>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run tests in an x86 process on 64 bit systems.
        /// </summary>
        /// <value>
        /// <c>true</c> to run tests in an x86 process on 64 bit systems; otherwise, <c>false</c>.
        /// </value>
        public bool X86 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Dispose each test runner after it has finished
        /// running its tests.
        /// </summary>
        /// <value>
        /// <c>true</c> to Dispose each test runner after it has finished
        /// running its tests; otherwise, <c>false</c>.
        /// </value>
        public bool DisposeRunners { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to shadow copy tests.
        /// Default value is <c>false</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if tests should be run as a shadow copy; otherwise, <c>false</c>.
        /// </value>
        public bool ShadowCopy { get; set; }

        /// <summary>
        /// Gets or sets the version of the runtime to be used when executing tests.
        /// </summary>
        /// <value>
        /// The version of the runtime to be used when executing tests.
        /// </value>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how NUnit should load tests in processes.
        /// The Default value is <see cref="NUnit3ProcessOption.Multiple"/>.
        /// </summary>
        public NUnit3ProcessOption Process { get; set; }

        /// <summary>
        /// Gets or sets a value to control creation of AppDomains for running tests.
        /// Corresponds to the /domain command line switch.
        /// The default is to use multiple domains if multiple assemblies are listed on the command line,
        /// otherwise a single domain is used.
        /// </summary>
        public NUnit3AppDomainUsage AppDomainUsage { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of test assembly agents to run at one
        /// time. If not specified, there is no limit.
        /// </summary>
        /// <value>
        /// The maximum number of test assembly agents to run at one time.
        /// </value>
        public int? Agents { get; set; }
    }
}