namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents a MSBuild platform target.
    /// </summary>
    public enum PlatformTarget
    {
        /// <summary>
        /// PlatformTarget: <c>MSIL</c> (AnyCPU)
        /// </summary>
        MSIL = 0,

        /// <summary>
        /// PlatformTarget: <c>x86</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x86 = 1,

        /// <summary>
        /// PlatformTarget: <c>x64</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x64 = 2,

        /// <summary>
        /// PlatformTarget: <c>ARM</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ARM = 3,
    }
}