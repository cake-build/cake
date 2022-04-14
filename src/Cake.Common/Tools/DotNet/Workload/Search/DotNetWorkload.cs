// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Workload.Search
{
    /// <summary>
    /// Workload information.
    /// </summary>
    public class DotNetWorkload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetWorkload" /> class.
        /// </summary>
        /// <param name="id">The workload Id.</param>
        /// <param name="description">The workload description.</param>
        public DotNetWorkload(string id, string description)
        {
            Id = id;
            Description = description;
        }

        /// <summary>
        /// Gets the workload Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the workload description.
        /// </summary>
        public string Description { get; }
    }
}
