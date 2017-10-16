// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class VersionCommand : Command
    {
        private readonly IConsole _console;

        public VersionCommand(IConsole console)
        {
            _console = console;
        }

        public override Task<bool> ExecuteAsync(ICakeEngine engine, CakeHostOptions options)
        {
            _console.Write(typeof(HelpCommand).GetTypeInfo().Assembly.GetName().Version.ToString(3));
            return Task.FromResult(true);
        }
    }
}
