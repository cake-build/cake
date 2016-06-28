using System.Diagnostics;

namespace Cake.Core.Polyfill
{
    internal static class ProcessHelper
    {
        public static void SetEnvironmentVariable(ProcessStartInfo info, string key, string value)
        {
#if NETCORE
            info.Environment[key] = value;
#else
            info.EnvironmentVariables[key] = value;
#endif
        }
    }
}
