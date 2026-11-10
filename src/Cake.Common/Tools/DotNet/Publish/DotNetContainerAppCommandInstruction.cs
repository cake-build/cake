// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Contains how the app command is set in the container image, i.e. the <c>ContainerAppCommandInstruction</c> MSBuild property.
/// </summary>
public enum DotNetContainerAppCommandInstruction
{
    /// <summary>
    /// The app command is set as the default arguments (CMD), keeping the base image entrypoint.
    /// </summary>
    DefaultArgs,

    /// <summary>
    /// The app command is set as the entrypoint.
    /// </summary>
    Entrypoint,

    /// <summary>
    /// The app command is not set.
    /// </summary>
    None
}
