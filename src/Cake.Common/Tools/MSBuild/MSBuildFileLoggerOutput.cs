namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// the type of file logger output to generate
    /// </summary>
    public enum MSBuildFileLoggerOutput
    {
        /// <summary>
        /// show errors and warnings
        /// </summary>
        All = 0,

        /// <summary>
        /// show errors only
        /// </summary>
        ErrorsOnly = 1,

        /// <summary>
        /// show warnings only
        /// </summary>
        WarningsOnly = 2,
    }
}