using System;
using System.Diagnostics;

namespace Cake.Commands
{
    internal sealed class VersionCommand : ICommand
    {
        // Delegate factory used by Autofac.
        public delegate VersionCommand Factory();

        public void Execute(CakeOptions options)
        {
            Console.WriteLine("Version {0}", GetVersion());
            Console.WriteLine();
        }

        private static string GetVersion()
        {
            var assembly = typeof(CakeApplication).Assembly;
            return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        }
    }
}
