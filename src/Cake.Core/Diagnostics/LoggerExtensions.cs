namespace Cake.Core.Diagnostics
{
    public static class LoggerExtensions
    {
        public static void Error(this ILogger logger, string format, params object[] args)
        {
            Error(logger, Verbosity.Quiet, format, args);
        }

        public static void Error(this ILogger logger, Verbosity verbosity, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(verbosity, LogLevel.Error, format, args);
            }
        }

        public static void Warning(this ILogger logger, string format, params object[] args)
        {
            Warning(logger, Verbosity.Minimal, format, args);
        }

        public static void Warning(this ILogger logger, Verbosity verbosity, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(verbosity, LogLevel.Warning, format, args);
            }
        }

        public static void Information(this ILogger logger, string format, params object[] args)
        {
            Information(logger, Verbosity.Normal, format, args);
        }

        public static void Information(this ILogger logger, Verbosity verbosity, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(verbosity, LogLevel.Information, format, args);
            }
        }

        public static void Verbose(this ILogger logger, string format, params object[] args)
        {
            Verbose(logger, Verbosity.Verbose, format, args);
        }

        public static void Verbose(this ILogger logger, Verbosity verbosity, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(verbosity, LogLevel.Verbose, format, args);
            }
        }

        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            Debug(logger, Verbosity.Diagnostic, format, args);
        }

        public static void Debug(this ILogger logger, Verbosity verbosity, string format, params object[] args)
        {
            if (logger != null)
            {
                logger.Write(verbosity, LogLevel.Debug, format, args);
            }
        }
    }
}
