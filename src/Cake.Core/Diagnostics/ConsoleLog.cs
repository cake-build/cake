using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
