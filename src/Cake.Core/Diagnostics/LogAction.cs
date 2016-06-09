// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
