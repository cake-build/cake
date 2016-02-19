namespace Cake.Common.Build.ContinuaCI
{
    /// <summary>
    /// Provides the known values for Continua CI Message Types
    /// </summary>
    public enum ContinuaCIMessageType
    {
        /// <summary>
        /// Debug Message
        /// </summary>
        Debug,

        /// <summary>
        /// Success Message
        /// </summary>
        Success,

        /// <summary>
        /// Information Message
        /// </summary>
        Information,

        /// <summary>
        /// Warning Message
        /// </summary>
        Warning,

        /// <summary>
        /// Error Message
        /// </summary>
        Error,

        /// <summary>
        /// Fatal Message
        /// </summary>
        Fatal,
    }
}