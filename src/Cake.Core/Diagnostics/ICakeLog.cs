namespace Cake.Core.Diagnostics
{
    public interface ICakeLog
    {
        void Write(Verbosity verbosity, LogLevel level, string format, params object[] args);
    }
}
