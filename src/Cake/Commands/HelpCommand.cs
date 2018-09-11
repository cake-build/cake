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

        // Delegate factory used by Autofac.
        public delegate HelpCommand Factory();

        public HelpCommand(IConsole console)
        {
            _console = console;
        }

        public bool Execute(CakeOptions options)
        {
            if (options.HasError)
            {
                _console.WriteLine("For usage, use parameter --help");
            }
            else
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
                _console.WriteLine("                         ({0})",
                    string.Join(", ", Enum.GetNames(typeof(Verbosity))));
                _console.WriteLine("    --debug              Performs a debug.");
                _console.WriteLine("    --showdescription    Shows description about tasks.");
                _console.WriteLine("    --showtree           Shows the task dependency tree.");
                _console.WriteLine("    --dryrun             Performs a dry run.");
                _console.WriteLine("    --exclusive          Execute a single task without any dependencies.");
                _console.WriteLine("    --bootstrap          Download/install modules defined by #module directives");
                _console.WriteLine("    --version            Displays version information.");
                _console.WriteLine("    --info               Displays additional information about Cake execution.");
                _console.WriteLine("    --help               Displays usage information.");
                _console.WriteLine();
            }
            return true;
        }
    }
}
