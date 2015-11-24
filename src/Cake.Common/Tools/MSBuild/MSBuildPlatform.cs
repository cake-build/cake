namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents an MSBuild exe platform.
    /// </summary>
    public enum MSBuildPlatform
    {
        /// <summary>
        /// Will build using MSBuild version based on PlatformTarget/Host OS.
        /// </summary>
        Automatic = 0,

        /// <summary>
        /// MSBuildPlatform: <c>x86</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x86 = 1,

        /// <summary>
        /// MSBuildPlatform: <c>x64</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x64 = 2
    }
}