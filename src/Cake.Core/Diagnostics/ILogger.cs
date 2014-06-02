using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Core.Diagnostics
{
    public interface ILogger
    {
        void Write(Verbosity verbosity, LogLevel level, string format, params object[] args);
    }
}
