// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.AppVeyor.Data;
using Cake.Core.IO;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Represents a service that communicates with AppVeyor.
    /// </summary>
    public interface IAppVeyorProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on AppVeyor.; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnAppVeyor { get; }

        /// <summary>
        /// Gets the AppVeyor environment.
        /// </summary>
        /// <value>
        /// The AppVeyor environment.
        /// </value>
        AppVeyorEnvironmentInfo Environment { get; }

        /// <summary>
        /// Uploads an AppVeyor artifact.
        /// </summary>
        /// <param name="path">The file path of the artifact to upload.</param>
        void UploadArtifact(FilePath path);

        /// <summary>
        /// Uploads test results XML file to AppVeyor. Results type can be one of the following: mstest, xunit, nunit, nunit3, junit.
        /// </summary>
        /// <param name="path">The file path of the test results XML to upload.</param>
        /// <param name="resultsType">The results type. Can be mstest, xunit, nunit, nunit3 or junit.</param>
        void UploadTestResults(FilePath path, AppVeyorTestResultsType resultsType);

        /// <summary>
        /// Updates the build version.
        /// </summary>
        /// <param name="version">The new build version.</param>
        void UpdateBuildVersion(string version);
    }
}
