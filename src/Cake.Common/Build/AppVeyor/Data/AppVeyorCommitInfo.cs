// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor commit information for a current build.
    /// </summary>
    public sealed class AppVeyorCommitInfo : AppVeyorInfo
    {
        /// <summary>
        /// Gets commit ID (SHA).
        /// </summary>
        /// <value>
        ///   The commit ID (SHA).
        /// </value>
        public string Id
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_COMMIT"); }
        }

        /// <summary>
        /// Gets the commit author's name.
        /// </summary>
        /// <value>
        ///   The commit author's name.
        /// </value>
        public string Author
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_COMMIT_AUTHOR"); }
        }

        /// <summary>
        /// Gets the commit author's email address.
        /// </summary>
        /// <value>
        ///   The commit author's email address.
        /// </value>
        public string Email
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL"); }
        }

        /// <summary>
        /// Gets the commit date/time.
        /// </summary>
        /// <value>
        ///   The commit date/time.
        /// </value>
        public string Timestamp
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_COMMIT_TIMESTAMP"); }
        }

        /// <summary>
        /// Gets the commit message.
        /// </summary>
        /// <value>
        ///   The commit message.
        /// </value>
        public string Message
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_COMMIT_MESSAGE"); }
        }

        /// <summary>
        /// Gets the rest of commit message after line break (if exists).
        /// </summary>
        /// <value>
        ///   The rest of commit message after line break (if exists).
        /// </value>
        public string ExtendedMessage
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_COMMIT_MESSAGE_EXTENDED"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorCommitInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorCommitInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
