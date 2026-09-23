// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Rwx.Commands;
using Cake.Common.Build.Rwx.Data;

namespace Cake.Common.Build.Rwx
{
    /// <summary>
    /// Represents an RWX Provider.
    /// </summary>
    public interface IRwxProvider
    {
        /// <summary>
        /// Gets a value indicating whether this instance is running on RWX.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running on RWX; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     Information("Running on RWX");
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Information("Running on RWX");
        /// }
        /// else
        /// {
        ///     Information("Not running on RWX");
        /// }
        /// </code>
        /// </example>
        bool IsRunningOnRwx { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     var runId = BuildSystem.Rwx.Environment.Run.Id;
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     var runId = Rwx.Environment.Run.Id;
        /// }
        /// </code>
        /// </example>
        RwxEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the RWX commands surface, used to write output values and upload
        /// artifacts at task runtime.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Rwx.IsRunningOnRwx)
        /// {
        ///     BuildSystem.Rwx.Commands.SetValue("cake_version", "1.0.0");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Rwx.</para>
        /// <example>
        /// <code>
        /// if (Rwx.IsRunningOnRwx)
        /// {
        ///     Rwx.Commands.SetValue("cake_version", "1.0.0");
        /// }
        /// </code>
        /// </example>
        RwxCommands Commands { get; }
    }
}
