// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Build.MyGet
{
    /// <summary>
    /// Represents a MyGet provider.
    /// </summary>
    public interface IMyGetProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on MyGet.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on MyGet; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnMyGet { get; }

        /// <summary>
        /// Report a build problem to MyGet.
        /// </summary>
        /// <param name="description">Description of build problem.</param>
        void BuildProblem(string description);

        /// <summary>
        /// Allows setting an environment variable that can be used by a future process.
        /// </summary>
        /// <param name="name">Name of the parameter to set.</param>
        /// <param name="value">Value to assign to the parameter.</param>
        void SetParameter(string name, string value);

        /// <summary>
        /// Write a status message to the MyGet build log.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <param name="errorDetails">Error details if status is error.</param>
        void WriteStatus(string message, MyGetBuildStatus status, string errorDetails = null);

        /// <summary>
        /// Tells MyGet to change the current build number.
        /// </summary>
        /// <param name="buildNumber">The required build number.</param>
        void SetBuildNumber(string buildNumber);
    }
}
