// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics;
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
            _console.WriteLine();
            _console.WriteLine(@"             +##   #;;'");
            _console.WriteLine(@"             #;;#  .+;;;;+,");
            _console.WriteLine(@"             '+;;#;,+';;;;;'#.");
            _console.WriteLine(@"             ++'''';;;;;;;;;;# ;#;");
            _console.WriteLine(@"            ##';;;;++'+#;;;;;'.   `#:");
            _console.WriteLine(@"         ;#   '+'';;;;;;;;;'#`       #.");
            _console.WriteLine(@"      `#,        .'++;;;;;':..........#");
            _console.WriteLine(@"    '+      `.........';;;;':.........#");
            _console.WriteLine(@"   #..................+;;;;;':........#");
            _console.WriteLine(@"   #..................#';;;;;'+''''''.#");
            _console.WriteLine(@"   #.......,:;''''''''##';;;;;'+'''''#,");
            _console.WriteLine(@"   #''''''''''''''''''###';;;;;;+''''#");
            _console.WriteLine(@"   #''''''''''''''''''####';;;;;;#'''#");
            _console.WriteLine(@"   #''''''''''''''''''#####';;;;;;#''#");
            _console.WriteLine(@"   #''''''''''''''''''######';;;;;;#'#");
            _console.WriteLine(@"   #''''''''''''''''''#######';;;;;;##");
            _console.WriteLine(@"   #''''''''''''''''''########';;;;;;#");
            _console.WriteLine(@"   #''''''''''''++####+;#######';;;;;;#");
            _console.WriteLine(@"   #+####':,`             ,#####';;;;;;'");
            _console.WriteLine(@"                              +##'''''+.");
            _console.WriteLine(@"   ___      _          ___       _ _     _ ");
            _console.WriteLine(@"  / __\__ _| | _____  / __\_   _(_) | __| |");
            _console.WriteLine(@" / /  / _` | |/ / _ \/__\// | | | | |/ _` |");
            _console.WriteLine(@"/ /___ (_| |   <  __/ \/  \ |_| | | | (_| |");
            _console.WriteLine(@"\____/\__,_|_|\_\___\_____/\__,_|_|_|\__,_|");
            _console.WriteLine();
            _console.WriteLine(@"                             Version {0}", GetVersion());
            _console.WriteLine();

            return true;
        }

        private static string GetVersion()
        {
            var assembly = typeof(CakeApplication).Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
    }
}
