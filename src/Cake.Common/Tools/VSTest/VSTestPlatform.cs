namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Target platform architecture to be used for test execution.
    /// </summary>
    public enum VSTestPlatform
    {
        /// <summary>
        /// Use default platform architecture.
        /// </summary>
        Default,

        /// <summary>
        /// Platform architecture: <c>x86</c>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x86,

        /// <summary>
        /// Platform architecture: <c>x64</c>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x64,

        /// <summary>
        /// Platform architecture: <c>ARM</c>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ARM
    }
}