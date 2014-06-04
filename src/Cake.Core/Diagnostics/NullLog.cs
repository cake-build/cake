namespace Cake.Core.Diagnostics
{
    public sealed class NullLog : ICakeLog
    {
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
        }
    }
}
