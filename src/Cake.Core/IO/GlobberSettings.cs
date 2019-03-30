using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Contains settings used by the globber.
    /// </summary>
    public sealed class GlobberSettings
    {
        /// <summary>
        /// Gets or sets the predicate used to filter directories based on file system information.
        /// </summary>
        public Func<IDirectory, bool> Predicate { get; set; }

        /// <summary>
        /// Gets or sets the filter used to filter files based on file system information.
        /// </summary>
        public Func<IFile, bool> FilePredicate { get; set; }

        /// <summary>
        /// Gets or sets whether or not globbing is case sensitive or not.
        /// If not set, the default value for the operating system will be used.
        /// </summary>
        public bool? IsCaseSensitive { get; set; }
    }
}
