namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Delegate representing lazy log action.
    /// </summary>
    /// <param name="actionEntry">Proxy to log.</param>
    public delegate void LogAction(LogActionEntry actionEntry);

    /// <summary>
    /// Delegate representing lazy log entry.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public delegate void LogActionEntry(string format, params object[] args);
}