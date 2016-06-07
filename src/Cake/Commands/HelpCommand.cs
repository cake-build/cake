// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Commands
{
    /// <summary>
    /// A command that displays help information.
    /// </summary>
    internal sealed class HelpCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly ICakeEnvironment _environment;

        // Delegate factory used by Autofac.
        public delegate HelpCommand Factory();

        public HelpCommand(IConsole console, ICakeEnvironment environment)
        {
            _console = console;
            _environment = environment;
        }

        public bool Execute(CakeOptions options)
        {
            _console.WriteLine();
            _console.WriteLine("Usage: Cake.exe [script] [--verbosity=value]");
            _console.WriteLine("                [--showdescription] [--dryrun] [..]");
            _console.WriteLine();
            _console.WriteLine("Example: Cake.exe");
            _console.WriteLine("Example: Cake.exe build.cake --verbosity=quiet");
            _console.WriteLine("Example: Cake.exe build.cake --showdescription");
            _console.WriteLine();
            _console.WriteLine("Options:");
            _console.WriteLine("    --verbosity=value    Specifies the amount of information to be displayed.");
            _console.WriteLine("                         ({0})", string.Join(", ", Enum.GetNames(typeof(Verbosity))));
            _console.WriteLine("    --debug              Performs a debug.");
            _console.WriteLine("    --showdescription    Shows description about tasks.");
            _console.WriteLine("    --dryrun             Performs a dry run.");
            _console.WriteLine("    --version            Displays version information.");
            _console.WriteLine("    --help               Displays usage information.");

            if (!_environment.IsUnix())
            {
                _console.WriteLine("    --mono               Uses the Mono Compiler, rather than Roslyn script engine.");
                _console.WriteLine("    --experimental       Uses the nightly builds of Roslyn script engine.");
            }

            _console.WriteLine();

            return true;
        }
    }
}
