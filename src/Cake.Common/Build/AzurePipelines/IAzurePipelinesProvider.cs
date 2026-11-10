// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.AzurePipelines.Data;

namespace Cake.Common.Build.AzurePipelines;

/// <summary>
/// Represents a Azure Pipelines provider.
/// </summary>
public interface IAzurePipelinesProvider
{
    /// <summary>
    /// Gets a value indicating whether the current build is running on Azure Pipelines.
    /// </summary>
    /// <value>
    /// <c>true</c> if the current build is running on Azure Pipelines; otherwise, <c>false</c>.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
    /// {
    ///     Information("Running on Azure Pipelines");
    /// }
    /// else
    /// {
    ///     Information("Not running on Azure Pipelines");
    /// }
    /// </code>
    /// </example>
    /// <para>Via AzurePipelines.</para>
    /// <example>
    /// <code>
    /// if (AzurePipelines.IsRunningOnAzurePipelines)
    /// {
    ///     Information("Running on Azure Pipelines");
    /// }
    /// else
    /// {
    ///     Information("Not running on Azure Pipelines");
    /// }
    /// </code>
    /// </example>
    bool IsRunningOnAzurePipelines { get; }

    /// <summary>
    /// Gets the Azure Pipelines environment.
    /// </summary>
    /// <value>
    /// The Azure Pipelines environment.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
    /// {
    ///     var definitionName = BuildSystem.AzurePipelines.Environment.BuildDefinition.Name;
    /// }
    /// </code>
    /// </example>
    /// <para>Via AzurePipelines.</para>
    /// <example>
    /// <code>
    /// if (AzurePipelines.IsRunningOnAzurePipelines)
    /// {
    ///     var definitionName = AzurePipelines.Environment.BuildDefinition.Name;
    /// }
    /// </code>
    /// </example>
    AzurePipelinesEnvironmentInfo Environment { get; }

    /// <summary>
    /// Gets the Azure Pipelines Commands provider.
    /// </summary>
    /// <value>
    /// The Azure Pipelines commands provider.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
    /// {
    ///     BuildSystem.AzurePipelines.Commands.WriteWarning("Watch this");
    /// }
    /// </code>
    /// </example>
    /// <para>Via AzurePipelines.</para>
    /// <example>
    /// <code>
    /// if (AzurePipelines.IsRunningOnAzurePipelines)
    /// {
    ///     AzurePipelines.Commands.WriteWarning("Watch this");
    /// }
    /// </code>
    /// </example>
    IAzurePipelinesCommands Commands { get; }
}
