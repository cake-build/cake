// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides optional data associated with an Azure Pipelines timeline record.
    /// </summary>
    public sealed class AzurePipelinesRecordData
    {
        /// <summary>
        /// Gets or sets the parent record of a new or existing timeline record.
        /// </summary>
        /// <value>
        /// The ID of the parent record.
        /// </value>
        public Guid ParentRecord { get; set; }

        /// <summary>
        /// Gets or sets the start time of this record.
        /// </summary>
        /// <value>
        /// The start time of this record.
        /// </value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the finish time of this record.
        /// </summary>
        /// <value>
        /// The finish time of this record.
        /// </value>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// Gets or sets the current progress of this record.
        /// </summary>
        /// <value>
        /// The current progress of this record.
        /// </value>
        public int? Progress { get; set; }

        /// <summary>
        /// Gets or sets the current status of this record.
        /// </summary>
        /// <value>
        /// The current status of this record.
        /// </value>
        public AzurePipelinesTaskStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the result of this record.
        /// </summary>
        /// <value>
        /// The result of this record.
        /// </value>
        public AzurePipelinesTaskResult? Result { get; set; }

        internal Dictionary<string, string> GetProperties()
        {
            var properties = new Dictionary<string, string>();
            if (ParentRecord != default(Guid))
            {
                properties.Add("parentid", ParentRecord.ToString());
            }
            if (StartTime.HasValue)
            {
                properties.Add("starttime", StartTime.ToString());
            }
            if (FinishTime.HasValue)
            {
                properties.Add("finishtime", FinishTime.ToString());
            }
            if (Progress.HasValue)
            {
                properties.Add("progress", Progress.ToString());
            }
            if (Status.HasValue)
            {
                properties.Add("state", Status.ToString());
            }
            if (Result.HasValue)
            {
                properties.Add("result", Result.ToString());
            }
            return properties;
        }
    }
}
