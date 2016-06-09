// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.ContinuaCI.Data;

namespace Cake.Common.Build.ContinuaCI
{
    /// <summary>
    /// Represents a Continua CI provider.
    /// </summary>
    public interface IContinuaCIProvider
    {
        /// <summary>
        /// Write a status message to the Continua CI build log.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        void WriteMessage(string message, ContinuaCIMessageType status);

        /// <summary>
        /// Write the start of a message group to the Continua CI build log.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        void WriteStartGroup(string groupName);

        /// <summary>
        /// Write the end of a message block to the Continua CI build log.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        void WriteEndBlock(string groupName);

        /// <summary>
        /// Set a Continua CI build variable.
        /// </summary>
        /// <param name="name">Name of the variable to set.</param>
        /// <param name="value">Value to assign to the variable.</param>
        /// <param name="skipIfNotDefined">Set to 'true' to prevent the build failing if the variable has not been defined for the configuration..</param>
        void SetVariable(string name, string value, bool skipIfNotDefined = true);

        /// <summary>
        /// Set a Continua CI build version.
        /// </summary>
        /// <param name="version">The new build version.</param>
        void SetBuildVersion(string version);

        /// <summary>
        /// Set a Continua CI build status message, which is shown on the build details page when a build is running.
        /// </summary>
        /// <param name="text">The new build status text.</param>
        void SetBuildStatus(string text);

        /// <summary>
        /// Gets a value indicating whether the current build is running on Continua CI.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Continua CI; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnContinuaCI { get; }

        /// <summary>
        /// Gets the Continua CI environment.
        /// </summary>
        /// <value>
        /// The Continua CI environment.
        /// </value>
        ContinuaCIEnvironmentInfo Environment { get; }
    }
}
