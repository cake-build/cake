// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;
using Cake.Core.Configuration;
using Spectre.Console.Cli;

namespace Cake.Infrastructure
{
    /// <summary>
    /// Responsible for registering all dependencies
    /// that is required for executing a script.
    /// </summary>
    public interface IContainerConfigurator
    {
        void Configure(
            ICakeContainerRegistrar registrar,
            ICakeConfiguration configuration,
            IRemainingArguments arguments);
    }
}
