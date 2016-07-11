// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about a <see cref="CakeTask"/> before its invocation.
    /// </summary>
    public interface ITaskSetupContext
    {
        /// <summary>
        /// Gets the <see cref="ICakeTaskInfo"/> describing the <see cref="CakeTask"/> that has just been invoked.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        ICakeTaskInfo Task { get; }
    }
}
