﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCORE
using Cake.Core.Scripting;

namespace Cake.Scripting.Mono
{
    /// <summary>
    /// Mono script host proxy.
    /// </summary>
    public class MonoScriptHostProxy
    {
        /// <summary>
        /// Gets or sets the script host.
        /// </summary>
        /// <value>The script host.</value>
        public static IScriptHost ScriptHost { get; set; }
    }
}
#endif