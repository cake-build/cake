// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Publish;

/// <summary>
/// Contains the container image manifest formats, i.e. the <c>ContainerImageFormat</c> MSBuild property.
/// </summary>
public enum DotNetContainerImageFormat
{
    /// <summary>
    /// Docker image manifest format.
    /// </summary>
    Docker,

    /// <summary>
    /// Open Container Initiative (OCI) image manifest format.
    /// </summary>
    OCI
}
