namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Represents a log level.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 0,
        
        /// <summary>
        /// Error
        /// </summary>
        Error = 1,
        
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 2,
        
        /// <summary>
        /// Information
        /// </summary>
        Information = 3,
        
        /// <summary>
        /// Verbose
        /// </summary>
        Verbose = 4,
        
        /// <summary>
        /// Debug
        /// </summary>
        Debug = 5
    }
}