﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Represents an entry in a <see cref="CakeReport"/>.
    /// </summary>
    public sealed class CakeReportEntry
    {
        /// <summary>
        /// Gets the task name.
        /// </summary>
        /// <value>The name.</value>
        public string TaskName { get; }

        /// <summary>
        /// Gets the duration the task ran for.
        /// </summary>
        /// <value>The duration the task ran for.</value>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets the task execution status.
        /// </summary>
        /// <value>The execution status.</value>
        public CakeTaskExecutionStatus ExecutionStatus { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportEntry"/> class.
        /// </summary>
        /// <param name="taskName">The name of the task.</param>
        /// <param name="duration">The duration.</param>
        public CakeReportEntry(string taskName, TimeSpan duration)
            : this(taskName, duration, CakeTaskExecutionStatus.Executed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportEntry"/> class.
        /// </summary>
        /// <param name="taskName">The name of the task.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="executionStatus">The execution status.</param>
        public CakeReportEntry(string taskName, TimeSpan duration, CakeTaskExecutionStatus executionStatus)
        {
            TaskName = taskName;
            Duration = duration;
            ExecutionStatus = executionStatus;
        }
    }
}