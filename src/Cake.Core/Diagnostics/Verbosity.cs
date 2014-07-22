using System.ComponentModel;

namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Represents verbosity.
    /// </summary>
    [TypeConverter(typeof(VerbosityTypeConverter))]
    public enum Verbosity
    {
        /// <summary>
        /// Quiet
        /// </summary>
        Quiet = 0,

        /// <summary>
        /// Minimal
        /// </summary>
        Minimal = 1,

        /// <summary>
        /// Normal
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Verbose
        /// </summary>
        Verbose = 3,

        /// <summary>
        /// Diagnostic
        /// </summary>
        Diagnostic = 4 
    }
}