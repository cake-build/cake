namespace Cake.Core.Diagnostics
{
    public sealed class NullLog : ILogger
    {
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
        }
    }
}
