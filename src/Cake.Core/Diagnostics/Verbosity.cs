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
        /// Quiet verbosity.
        /// </summary>
        Quiet = 0,

        /// <summary>
        /// Minimal verbosity.
        /// </summary>
        Minimal = 1,

        /// <summary>
        /// Normal verbosity.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Verbose verbosity.
        /// </summary>
        Verbose = 3,

        /// <summary>
        /// Diagnostic verbosity.
        /// </summary>
        Diagnostic = 4 
    }
}