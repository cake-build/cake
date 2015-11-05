namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// Represents XUnit2's options for parallel test execution
    /// </summary>
    public enum ParallelismOption
    {
        /// <summary>
        /// Turn off all parallelization
        /// </summary>
        None,

        /// <summary>
        /// Only parallelize collections
        /// </summary>
        Collections,

        /// <summary>
        /// Only parallelize assemblies
        /// </summary>
        Assemblies,

        /// <summary>
        /// Parallelize assemblies and collections.
        /// </summary>
        All
    }
}