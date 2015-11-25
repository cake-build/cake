namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Represents the various ways NUnit loads tests in processes.
    /// </summary>
    public enum NUnit3ProcessOption
    {
        /// <summary>
        /// A separate process is created for each test assembly. This is the default.
        /// </summary>
        Multiple = 0,

        /// <summary>
        /// One separate process is created to run all of the test assemblies.
        /// </summary>
        Separate,

        /// <summary>
        /// All the tests are run in the nunit-console process.
        /// </summary>
        InProcess
    }
}