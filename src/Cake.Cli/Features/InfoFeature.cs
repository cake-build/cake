// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using System;

namespace Cake.Cli
{
    /// <summary>
    /// Represents a feature that writes information about Cake to the console.
    /// </summary>
    public interface ICakeInfoFeature
    {
        /// <summary>
        /// Runs the feature.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        void Run(IConsole console);
    }

    /// <summary>
    /// Writes information about Cake to the console.
    /// </summary>
    public sealed class InfoFeature : ICakeInfoFeature
    {
        private readonly IVersionResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoFeature"/> class.
        /// </summary>
        /// <param name="resolver">The version resolver.</param>
        public InfoFeature(IVersionResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc/>
        public void Run(IConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            var version = _resolver.GetVersion();
            var product = _resolver.GetProductVersion();

            console.WriteLine();
            console.WriteLine(@"             +##   #;;'");
            console.WriteLine(@"             #;;#  .+;;;;+,");
            console.WriteLine(@"             '+;;#;,+';;;;;'#.");
            console.WriteLine(@"             ++'''';;;;;;;;;;# ;#;");
            console.WriteLine(@"            ##';;;;++'+#;;;;;'.   `#:");
            console.WriteLine(@"         ;#   '+'';;;;;;;;;'#`       #.");
            console.WriteLine(@"      `#,        .'++;;;;;':..........#");
            console.WriteLine(@"    '+      `.........';;;;':.........#");
            console.WriteLine(@"   #..................+;;;;;':........#");
            console.WriteLine(@"   #..................#';;;;;'+''''''.#");
            console.WriteLine(@"   #.......,:;''''''''##';;;;;'+'''''#,");
            console.WriteLine(@"   #''''''''''''''''''###';;;;;;+''''#");
            console.WriteLine(@"   #''''''''''''''''''####';;;;;;#'''#");
            console.WriteLine(@"   #''''''''''''''''''#####';;;;;;#''#");
            console.WriteLine(@"   #''''''''''''''''''######';;;;;;#'#");
            console.WriteLine(@"   #''''''''''''''''''#######';;;;;;##");
            console.WriteLine(@"   #''''''''''''''''''########';;;;;;#");
            console.WriteLine(@"   #''''''''''''++####+;#######';;;;;;#");
            console.WriteLine(@"   #+####':,`             ,#####';;;;;;'");
            console.WriteLine(@"                              +##'''''+.");

            console.ForegroundColor = System.ConsoleColor.Yellow;
            console.WriteLine(@"   ___      _          ___       _ _     _ ");
            console.WriteLine(@"  / __\__ _| | _____  / __\_   _(_) | __| |");
            console.WriteLine(@" / /  / _` | |/ / _ \/__\// | | | | |/ _` |");
            console.WriteLine(@"/ /___ (_| |   <  __/ \/  \ |_| | | | (_| |");
            console.WriteLine(@"\____/\__,_|_|\_\___\_____/\__,_|_|_|\__,_|");
            console.ResetColor();

            console.WriteLine();
            console.WriteLine(@"Version: {0}", version);
            console.WriteLine(@"Details: {0}", string.Join("\n         ", product.Split('/')));
            console.WriteLine();
        }
    }
}
