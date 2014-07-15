using System.Diagnostics;
using Cake.Core;
using Cake.Core.Extensions;

namespace Cake.Commands
{
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
            _console.WriteLine("Version {0}", GetVersion());
            _console.WriteLine();
        }

        private static string GetVersion()
        {
            var assembly = typeof(CakeApplication).Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
    }
}
