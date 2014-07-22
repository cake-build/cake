namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Represents an ILMerge target.
    /// </summary>
    public enum TargetKind
    {
        /// <summary>
        /// The default target.
        /// </summary>
        Default,

        /// <summary>
        /// Dynamic Link Library
        /// </summary>
        Dll,

        /// <summary>
        /// Executable
        /// </summary>
        Exe,

        /// <summary>
        /// Windows executable
        /// </summary>
        WinExe
    }
}