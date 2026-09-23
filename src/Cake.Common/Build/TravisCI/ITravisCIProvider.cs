// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TravisCI.Data;

namespace Cake.Common.Build.TravisCI
{
    /// <summary>
    /// Represents a Travis CI Provider.
    /// </summary>
    public interface ITravisCIProvider
    {
        /// <summary>
        /// Gets a value indicating whether this instance is running on Travis CI.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running on Travis CI; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information("Running on Travis CI");
        /// }
        /// else
        /// {
        ///     Information("Not running on Travis CI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information("Running on Travis CI");
        /// }
        /// else
        /// {
        ///     Information("Not running on Travis CI");
        /// }
        /// </code>
        /// </example>
        bool IsRunningOnTravisCI { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     var osName = BuildSystem.TravisCI.Environment.Job.OSName;
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     var osName = TravisCI.Environment.Job.OSName;
        /// }
        /// </code>
        /// </example>
        TravisCIEnvironmentInfo Environment { get; }

        /// <summary>
        /// Write the start of a message fold to the Travis CI build log.
        /// </summary>
        /// <param name="name">Name of the group.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     BuildSystem.TravisCI.WriteStartFold("restore");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     TravisCI.WriteStartFold("restore");
        /// }
        /// </code>
        /// </example>
        void WriteStartFold(string name);

        /// <summary>
        /// Write the start of a message fold to the Travis CI build log.
        /// </summary>
        /// <param name="name">Name of the group.</param>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     BuildSystem.TravisCI.WriteEndFold("restore");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     TravisCI.WriteEndFold("restore");
        /// }
        /// </code>
        /// </example>
        void WriteEndFold(string name);
    }
}