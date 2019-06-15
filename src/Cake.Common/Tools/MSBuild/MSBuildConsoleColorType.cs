﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents how the console color should behave in a console.
    /// </summary>
    public enum MSBuildConsoleColorType
    {
        /// <summary>
        /// Use the normal console color behaviour.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Use the default console color for all logging messages.
        /// </summary>
        Disabled,

        /// <summary>
        /// Use ANSI console colors even if console does not support it
        /// </summary>
        ForceAnsi
    }
}
