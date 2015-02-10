using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    /// <summary>
    /// Represents a verbosity aware log.
    /// </summary>
    public interface IVerbosityAwareLog : ICakeLog
    {
        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        Verbosity Verbosity { get; set; }
    }
}
