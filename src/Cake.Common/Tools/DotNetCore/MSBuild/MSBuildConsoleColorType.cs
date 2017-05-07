namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Represents how the console color should behave in a console.
    /// </summary>
    public enum MSBuildConsoleColorType
    {
        /// <summary>
        /// Use the normal console color behaviour.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Use the default console color for all logging messages.
        /// </summary>
        Disabled,

        /// <summary>
        /// Use ANSI console colors even if console does not support it
        /// </summary>
        ForceAnsi
    }
}
