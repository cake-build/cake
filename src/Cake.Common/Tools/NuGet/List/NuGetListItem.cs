namespace Cake.Common.Tools.NuGet.List
{
    /// <summary>
    /// An item as returned by <see cref="NuGetList"/>.
    /// </summary>
    public sealed class NuGetListItem
    {
        /// <summary>
        /// Gets or sets the name of the NuGetListItem.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version of the NuGetListItem as string.
        /// </summary>
        public string Version { get; set; }
    }
}
