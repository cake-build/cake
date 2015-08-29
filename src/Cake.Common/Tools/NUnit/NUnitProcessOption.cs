namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    ///     Represents the various ways NUnit loads tests in processes.
    /// </summary>
    public enum NUnitProcessOption
    {
        /// <summary>
        ///     All the tests are run in the nunit-console process. This is the default.
        /// </summary>
        Single,

        /// <summary>
        ///     A separate process is created to run the tests.
        /// </summary>
        Separate,

        /// <summary>
        ///     A separate process is created for each test assembly.
        /// </summary>
        Multiple
    }
}