namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Target .NET Framework version to be used for test execution.
    /// </summary>
    public enum VSTestFrameworkVersion
    {
        /// <summary>
        /// Use default .NET Framework version.
        /// </summary>
        Default,

        /// <summary>
        /// .NET Framework version: <c>3.5</c>.
        /// </summary>
        NET35,

        /// <summary>
        /// .NET Framework version: <c>4.0</c>.
        /// </summary>
        NET40,

        /// <summary>
        /// .NET Framework version: <c>4.5</c>.
        /// </summary>
        NET45
    }
}