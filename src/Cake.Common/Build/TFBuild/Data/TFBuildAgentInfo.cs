// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.TFBuild.Data
{
    /// <summary>
    /// Provides TF Build agent info for the current build and build agent
    /// </summary>
    public sealed class TFBuildAgentInfo : TFInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TFBuildAgentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TFBuildAgentInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the local path on the agent where all folders for a given build definition are created.
        /// </summary>
        /// <value>
        /// The local path on the agent where all folders for a given build definition are created.
        /// </value>
        /// <example><c>c:\agent\_work\1</c></example>
        public FilePath BuildDirectory => GetEnvironmentString("AGENT_BUILDDIRECTORY");

        /// <summary>
        /// Gets the directory the agent is installed into. This contains the agent software.
        /// </summary>
        /// <remarks>If you are using an on-premises agent, this directory is specified by you.</remarks>
        /// <value>
        /// The directory the agent is installed into.
        /// </value>
        /// <example><c>c:\agent\</c></example>
        public FilePath HomeDirectory => GetEnvironmentString("AGENT_HOMEDIRECTORY");

        /// <summary>
        /// Gets the working directory for this agent.
        /// </summary>
        /// <value>
        /// The working directory for this agent.
        /// </value>
        public FilePath WorkingDirectory => GetEnvironmentString("AGENT_WORKFOLDER");

        /// <summary>
        /// Gets the ID of the agent.
        /// </summary>
        /// <value>
        /// The ID of the agent.
        /// </value>
        public int Id => GetEnvironmentInteger("AGENT_ID");

        /// <summary>
        /// Gets the name of the agent that is registered with the pool.
        /// </summary>
        /// <remarks>If you are using an on-premises agent, this is specified by you.</remarks>
        /// <value>
        /// The name of the agent that is registered with the pool.
        /// </value>
        public string Name => GetEnvironmentString("AGENT_NAME");

        /// <summary>
        /// Gets the name of the machine on which the agent is installed.
        /// </summary>
        /// <value>
        /// The name of the machine on which the agent is installed.
        /// </value>
        public string MachineName => GetEnvironmentString("AGENT_MACHINE_NAME");

        /// <summary>
        /// Gets a value indicating whether the current agent is a hosted agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current agent is a hosted agent; otherwise, <c>false</c>.
        /// </value>
        public bool IsHosted => Name == "Hosted Agent";
    }
}