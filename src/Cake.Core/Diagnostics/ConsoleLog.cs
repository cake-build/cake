using System;

namespace Cake.Core.Diagnostics
{
    public sealed class ConsoleLog : ILogger
    {
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
