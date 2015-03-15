using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    /// <summary>
    /// Represents a verbosity aware log.
    /// </summary>
    public interface IVerbosityAwareLog : ICakeLog
    {
        /// <summary>
        /// Sets the verbosity.
        /// </summary>
        /// <param name="verbosity">The desired verbosity.</param>
        void SetVerbosity(Verbosity verbosity);
    }
}
