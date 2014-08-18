using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains settings used by <see cref="NUnitRunner"/>.
    /// </summary>
    public sealed class NUnitSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>
        /// The tool path.
        /// Defaults to './tools/**/nunit-console.exe'.
        /// </value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Specifies the name of the Xml Results File.
        /// </summary>
        /// <value>
        /// Results file path.
        /// Defaults to 'TestResult.xml'
        /// </value>
        public FilePath ResultsFile { get; set; }

        /// <summary>
        /// Specifies wether to generate the results xml file.
        /// </summary>
        public bool NoResults { get; set; }

        /// <summary>
        /// Allows you to specify the version of the runtime to be used in executing tests.
        /// </summary>
        /// <remarks>
        /// If that version specified is different from the one being used by NUnit, the tests are run in a separate process.
        /// </remarks>
        public string Framework { get; set; }

        /// <summary>
        /// Specifies categories to include in running. 
        /// See <see cref="!:http://nunit.org/index.php?p=consoleCommandLine&amp;r=2.6.3 "> examples here</see> under "Specifying Test Categories to Include or Exclude".
        /// </summary>
        public string Include { get; set; }

        /// <summary>
        /// Specifies categories to exclude from running.
        /// See <see cref="!:http://nunit.org/index.php?p=consoleCommandLine&amp;r=2.6.3 ">examples here</see> under "Specifying Test Categories to Include or Exclude".
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        /// Represents the default timeout to be used for test cases in this run. 
        /// If any test exceeds the timeout value, it is cancelled and reported as an error.
        /// </summary>
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
        /// Suppresses use of a separate thread for running the tests and uses the main thread instead.
        /// </summary>
        public bool NoThread { get; set; }

        /// <summary>
        /// Disables display of the copyright information at the start of the program.
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Causes execution of the test run to terminate immediately on the first test failure or error.
        /// </summary>
        public bool StopOnError { get; set; }

        /// <summary>
        /// Allows you to control the amount of information that NUnit writes to its internal trace log.
        /// </summary>
        /// <value>
        ///     Valid values are Off, Error, Warning, Info, and Debug. The default is Off.
        /// </value>
        public string Trace { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitSettings"/> class.
        /// </summary>
        public NUnitSettings()
        {
            ShadowCopy = true;
        }
    }
}
