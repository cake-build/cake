// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Features.Introspection
{
    public interface IInfoFeature
    {
        int Run();
    }

    public sealed class InfoFeature : IInfoFeature
    {
        private readonly IConsole _console;
        private readonly IVersionResolver _resolver;

        public InfoFeature(IConsole console, IVersionResolver resolver)
        {
            _console = console;
            _resolver = resolver;
        }

        public int Run()
        {
            var version = _resolver.GetVersion();
            var product = _resolver.GetProductVersion();

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

            _console.ForegroundColor = System.ConsoleColor.Yellow;
            _console.WriteLine(@"   ___      _          ___       _ _     _ ");
            _console.WriteLine(@"  / __\__ _| | _____  / __\_   _(_) | __| |");
            _console.WriteLine(@" / /  / _` | |/ / _ \/__\// | | | | |/ _` |");
            _console.WriteLine(@"/ /___ (_| |   <  __/ \/  \ |_| | | | (_| |");
            _console.WriteLine(@"\____/\__,_|_|\_\___\_____/\__,_|_|_|\__,_|");
            _console.ResetColor();

            _console.WriteLine();
            _console.WriteLine(@"Version: {0}", version);
            _console.WriteLine(@"Details: {0}", string.Join("\n         ", product.Split('/')));
            _console.WriteLine();

            return 0;
        }
    }
}
