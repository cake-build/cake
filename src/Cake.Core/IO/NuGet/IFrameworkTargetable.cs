using System.Collections.Generic;
using System.Runtime.Versioning;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Represents an item that may support specific .Net framework versions
    /// </summary>
    public interface IFrameworkTargetable
    {
        /// <summary>
        /// Gets the frameworks supported by this item.
        /// </summary>
        /// <value>
        /// The supported frameworks.
        /// </value>
        IEnumerable<FrameworkName> SupportedFrameworks { get; }
    }
}