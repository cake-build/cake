// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class HelpCommand : Command
    {
        private readonly IConsole _console;

        public HelpCommand(IConsole console)
        {
            _console = console;
        }

        public override Task<bool> ExecuteAsync(ICakeEngine engine, CakeHostOptions options)
        {
            _console.Write("Cake.Frosting (");
            _console.ForegroundColor = ConsoleColor.Yellow;
            _console.Write(typeof(HelpCommand).GetTypeInfo().Assembly.GetName().Version.ToString(3));
            _console.ResetColor();
            _console.WriteLine(")");

            _console.WriteLine("Usage:");
            _console.WriteLine("  dotnet {0}.dll [options]", typeof(HelpCommand).GetTypeInfo().Assembly.GetName().Name);
            _console.WriteLine();
            _console.WriteLine("Options:");
            _console.WriteLine("  --target|-t <TARGET>          Sets the build target");
            _console.WriteLine("  --working|-w <DIR>            Sets the working directory");
            _console.WriteLine("  --verbosity|-v <VERBOSITY>    Sets the verbosity");
            _console.WriteLine("  --dryrun|-r                   Performs a dry run");
            _console.WriteLine("  --version                     Displays Cake.Frosting version number");
            _console.WriteLine("  --help|-h                     Show help");
            _console.WriteLine();

            return Task.FromResult(true);
        }
    }
}
