// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Common.Build
{
    /// <summary>
    /// Represents a build provider.
    /// </summary>
    [Flags]
    public enum BuildProvider
    {
        /// <summary>
        /// Local build provider.
        /// </summary>
        Local = 0,

        /// <summary>
        /// AppVeyor build provider.
        /// </summary>
        AppVeyor = 1,

        /// <summary>
        /// TeamCity build provider.
        /// </summary>
        TeamCity = 2,

        /// <summary>
        /// MyGet build provider.
        /// </summary>
        MyGet = 4,

        /// <summary>
        /// Bamboo build provider.
        /// </summary>
        Bamboo = 8,

        /// <summary>
        /// ContinuaCI build provider.
        /// </summary>
        ContinuaCI = 16,

        /// <summary>
        /// Jenkins build provider.
        /// </summary>
        Jenkins = 32,

        /// <summary>
        /// Bitrise build provider.
        /// </summary>
        Bitrise = 64,

        /// <summary>
        /// TravisCI build provider.
        /// </summary>
        TravisCI = 128,

        /// <summary>
        /// BitbucketPipelines build provider.
        /// </summary>
        BitbucketPipelines = 256,

        /// <summary>
        /// GoCD build provider.
        /// </summary>
        GoCD = 512,

        /// <summary>
        /// GitLabCI build provider.
        /// </summary>
        GitLabCI = 1024,

        /// <summary>
        /// AzurePipelines build provider.
        /// </summary>
        AzurePipelines = 2048,

        /// <summary>
        /// GitHubActions build provider.
        /// </summary>
        GitHubActions = 8192
    }
}