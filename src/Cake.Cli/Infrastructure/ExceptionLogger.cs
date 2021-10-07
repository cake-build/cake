using System;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Cli
{
    /// <summary>
    /// Exception logging extension methods for the ICakeLog.
    /// </summary>
    public static class ExceptionLogger
    {
        /// <summary>
        /// Logs exception and returns exit code if available in exception.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="log">The log.</param>
        /// <param name="ex">The exception.</param>
        /// <returns>1 or exit code provided by <see cref="CakeException.ExitCode"/>.</returns>
        public static int LogException<T>(this ICakeLog log, T ex)
            where T : Exception
        {
            log = log ?? new CakeBuildLog(
                new CakeConsole(new CakeEnvironment(new CakePlatform(), new CakeRuntime())));

            if (log.Verbosity == Verbosity.Diagnostic)
            {
                log.Error("Error: {0}", ex);
            }
            else
            {
                log.Error("Error: {0}", ex.Message);
                if (ex is AggregateException aex)
                {
                    foreach (var exception in aex.Flatten().InnerExceptions)
                    {
                        log.Error("\t{0}", exception.Message);
                    }
                }
            }

            var exitCode = ex switch
            {
                CakeException cex => cex.ExitCode,
                AggregateException aex => aex
                                            .InnerExceptions
                                            .OfType<CakeException>()
                                            .Select(cex => cex.ExitCode as int?)
                                            .FirstOrDefault() ?? 1,
                _ => 1
            };

            return exitCode;
        }
    }
}
