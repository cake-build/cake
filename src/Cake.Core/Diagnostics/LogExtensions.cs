namespace Cake.Core.Diagnostics
{
    public static class LogExtensions
    {
        public static void Error(this ICakeLog log, string format, params object[] args)
        {
            Error(log, Verbosity.Quiet, format, args);
        }

        public static void Error(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Error, format, args);
            }
        }

        public static void Warning(this ICakeLog log, string format, params object[] args)
        {
            Warning(log, Verbosity.Minimal, format, args);
        }

        public static void Warning(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Warning, format, args);
            }
        }

        public static void Information(this ICakeLog log, string format, params object[] args)
        {
            Information(log, Verbosity.Normal, format, args);
        }

        public static void Information(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Information, format, args);
            }
        }

        public static void Verbose(this ICakeLog log, string format, params object[] args)
        {
            Verbose(log, Verbosity.Verbose, format, args);
        }

        public static void Verbose(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Verbose, format, args);
            }
        }

        public static void Debug(this ICakeLog log, string format, params object[] args)
        {
            Debug(log, Verbosity.Diagnostic, format, args);
        }

        public static void Debug(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Debug, format, args);
            }
        }
    }
}
