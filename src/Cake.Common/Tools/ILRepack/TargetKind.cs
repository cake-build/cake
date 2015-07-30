namespace Cake.Common.Tools.ILRepack
{
    /// <summary>
    /// Represents an ILMerge target.
    /// </summary>
    public enum TargetKind
    {
        /// <summary>
        /// TargetKind: <c>Default</c>
        /// </summary>
        Default,

        /// <summary>
        /// TargetKind: <c>Dynamic Link Library</c>
        /// </summary>
        Dll,

        /// <summary>
        /// TargetKind: <c>Executable</c>
        /// </summary>
        Exe,

        /// <summary>
        /// TargetKind: <c>Windows executable</c>
        /// </summary>
        WinExe
    }
}