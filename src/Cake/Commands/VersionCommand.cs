// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Reflection;
using Cake.Core;

namespace Cake.Commands
{
    /// <summary>
    /// A command that shows version information.
    /// </summary>
    internal sealed class VersionCommand : ICommand
    {
        private readonly IConsole _console;

        // Delegate factory used by Autofac.
        public delegate VersionCommand Factory();

        public VersionCommand(IConsole console)
        {
            _console = console;
        }

        public bool Execute(CakeOptions options)
        {
            _console.WriteLine(@"{0}", GetVersion());
            return true;
        }

        private static string GetVersion()
        {
            var assembly = typeof(CakeApplication).GetTypeInfo().Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
    }
}
