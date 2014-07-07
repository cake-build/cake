using System;

namespace Cake.Commands
{
    internal sealed class HelpCommand : ICommand
    {
        // Delegate factory used by Autofac.
        public delegate HelpCommand Factory();

        public void Execute(CakeOptions options)
        {
            Console.WriteLine("Usage: Cake.exe <build-script> [-verbosity=value] [-showdescription] [..]");
            Console.WriteLine();
            Console.WriteLine("Example: Cake.exe build.cake");
            Console.WriteLine("Example: Cake.exe build.cake -verbosity=quiet");
            Console.WriteLine("Example: Cake.exe build.cake -showdescription");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("    -verbosity=value    Specifies the amount of information to be displayed.");
            Console.WriteLine("    -showdescription    Shows description about tasks.");
            Console.WriteLine("    -help               Displays usage information.");
        }
    }
}
