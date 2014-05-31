using System.Diagnostics;

namespace Cake.Core.IO
{
    public interface IProcessRunner
    {
        IProcess Start(ProcessStartInfo info);
    }
}
