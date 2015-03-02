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

        public void Execute(CakeOptions options)
        {
            _console.WriteLine(@"             +##   #;;'+
             #;;#  .+;;;;+,
             '+;;#;,+';;;;;'#.
             ++'''';;;;;;;;;;# ;#;
            ##';;;;++'+#;;;;;'.   `#:
         ;#   '+'';;;;;;;;;'#`       #.
      `#,        .'++;;;;;':..........#
    '+      `.........';;;;':.........#
   #..................+;;;;;':........#
   #..................#';;;;;'+''''''.#
   #.......,:;''''''''##';;;;;'+'''''#,
   #''''''''''''''''''###';;;;;;+''''#
   #''''''''''''''''''####';;;;;;#'''#
   #''''''''''''''''''#####';;;;;;#''#
   #''''''''''''''''''######';;;;;;#'#
   #''''''''''''''''''#######';;;;;;##
   #''''''''''''''''''########';;;;;;#
   #''''''''''''++####+;#######';;;;;;#
   #+####':,`             ,#####';;;;;;'
                              +##'''''+.
   ___      _          ___       _ _     _ 
  / __\__ _| | _____  / __\_   _(_) | __| |
 / /  / _` | |/ / _ \/__\// | | | | |/ _` |
/ /___ (_| |   <  __/ \/  \ |_| | | | (_| |
\____/\__,_|_|\_\___\_____/\__,_|_|_|\__,_|
                            Version {0}",
               GetVersion());
            _console.WriteLine();
        }

        private static string GetVersion()
        {
            var assembly = typeof(CakeApplication).Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
    }
}
