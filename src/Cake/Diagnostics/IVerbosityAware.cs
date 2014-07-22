using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    public interface IVerbosityAwareLog : ICakeLog
    {
        Verbosity Verbosity { get; set; }
    }
}
