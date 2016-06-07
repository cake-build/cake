namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Provides the known values for the AppVeyor test results types.
    /// </summary>
    public enum AppVeyorTestResultsType
    {
        /// <summary>
        /// MSTest test results.
        /// </summary>
        MSTest,

        /// <summary>
        /// XUnit test results.
        /// </summary>
        XUnit,

        /// <summary>
        /// NUnit test results.
        /// </summary>
        NUnit,

        /// <summary>
        /// NUnit v3 test results.
        /// </summary>
        NUnit3,

        /// <summary>
        /// JUnit test results.
        /// </summary>
        JUnit
    }
}
