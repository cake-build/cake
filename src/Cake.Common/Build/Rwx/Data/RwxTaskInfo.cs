// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Rwx.Data
{
    /// <summary>
    /// Provides RWX task information for the current build.
    /// </summary>
    public sealed class RwxTaskInfo : RwxInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RwxTaskInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RwxTaskInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the RWX identifier of the current task.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public string Id => GetEnvironmentString("RWX_TASK_ID");

        /// <summary>
        /// Gets the link to the current task in RWX.
        /// </summary>
        /// <value>
        /// The task URL.
        /// </value>
        public string Url => GetEnvironmentString("RWX_TASK_URL");

        /// <summary>
        /// Gets the attempt number of the current RWX task.
        /// </summary>
        /// <value>
        /// The task attempt number.
        /// </value>
        public int AttemptNumber => GetEnvironmentInteger("RWX_TASK_ATTEMPT_NUMBER");
    }
}
