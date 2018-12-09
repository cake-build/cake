namespace Cake.Common.Tools.NuGet.List
{
    /// <summary>
    /// Define how to handle Comparison of package ID
    /// </summary>
    public enum PackageIdCompare
    {
        /// <summary>
        /// Package ID Contains a text to find
        /// </summary>
        Contains = 0,

        /// <summary>
        /// Package ID text to find must be equals
        /// </summary>
        Equals = 1,
    }
}
