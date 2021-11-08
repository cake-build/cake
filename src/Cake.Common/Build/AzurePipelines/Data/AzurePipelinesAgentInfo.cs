// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines agent info for the current build and build agent.
    /// </summary>
    public sealed class AzurePipelinesAgentInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesAgentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesAgentInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the local path on the agent where all folders for a given build definition are created.
        /// </summary>
        /// <value>
        /// The local path on the agent where all folders for a given build definition are created.
        /// </value>
        /// <example><c>c:\agent\_work\1</c>.</example>
        public FilePath BuildDirectory => GetEnvironmentString("AGENT_BUILDDIRECTORY");

        /// <summary>
        /// Gets the directory the agent is installed into. This contains the agent software.
        /// </summary>
        /// <remarks>If you are using a self-hosted agent, this directory is specified by you.</remarks>
        /// <value>
        /// The directory the agent is installed into.
        /// </value>
        /// <example><c>c:\agent\</c>.</example>
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
        /// Gets the display name of the running job.
        /// </summary>
        /// <value>
        /// The display name of the running job.
        /// </value>
        public string JobName => GetEnvironmentString("AGENT_JOBNAME");

        /// <summary>
        /// Gets the status of the build.
        /// </summary>
        /// <value>
        /// The status of the build.
        /// </value>
        public string JobStatus => GetEnvironmentString("AGENT_JOBSTATUS");

        /// <summary>
        /// Gets the name of the agent that is registered with the pool.
        /// </summary>
        /// <remarks>If you are using a self-hosted agent, this is specified by you.</remarks>
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
        public string MachineName => GetEnvironmentString("AGENT_MACHINENAME");

        /// <summary>
        /// Gets the directory used by tasks such as Node Tool Installer and Use Python Version to switch between multiple versions of a tool.
        /// </summary>
        /// <remarks>
        /// These tasks will add tools from this directory to PATH so that subsequent build steps can use them.
        /// </remarks>
        /// <value>
        /// The task directory.
        /// </value>
        public FilePath ToolsDirectory => GetEnvironmentString("AGENT_TOOLSDIRECTORY");

        /// <summary>
        /// Gets a value indicating whether the current agent is a Microsoft hosted agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current agent is a Microsoft hosted agent. <c>false</c> if the current agent is a self hosted agent.
        /// </value>
        public bool IsHosted => Name != null && (Name.StartsWith("Hosted") || Name.StartsWith("Azure Pipelines"));
    }
}