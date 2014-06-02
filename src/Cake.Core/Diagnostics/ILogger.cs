namespace Cake.Core.Diagnostics
{
    public interface ILogger
    {
        void Write(Verbosity verbosity, LogLevel level, string format, params object[] args);
    }
}
