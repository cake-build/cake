namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Loggers available for outputting test results.
    /// </summary>
    public enum VSTestLogger
    {
        /// <summary>
        /// No logging of test results.
        /// </summary>
        None,

        /// <summary>
        /// Log results to a Visual Studio test results file.
        /// </summary>
        Trx
    }
}