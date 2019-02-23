﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// The Git version output type.
    /// </summary>
    public enum GitVersionOutput
    {
        /// <summary>
        /// Outputs to the stdout using json.
        /// </summary>
        Json,

        /// <summary>
        /// Outputs to the stdout in a way usable by a detected build server.
        /// </summary>
        BuildServer
    }
}