// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using NuGet.Common;
using LogLevel = NuGet.Common.LogLevel;

namespace Cake.NuGet
{
    internal sealed class NuGetLogger : ILogger
    {
        private readonly ICakeLog _log;

        private static Core.Diagnostics.LogLevel GetLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Minimal:
                    return Core.Diagnostics.LogLevel.Information;
                case LogLevel.Warning:
                    return Core.Diagnostics.LogLevel.Warning;
                case LogLevel.Error:
                    return Core.Diagnostics.LogLevel.Error;
                default:
                    return Core.Diagnostics.LogLevel.Debug;
            }
        }

        private static Verbosity GetVerbosity(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Verbose:
                    return Verbosity.Diagnostic;
                case LogLevel.Warning:
                    return Verbosity.Minimal;
                case LogLevel.Error:
                    return Verbosity.Quiet;
                default:
                    return Verbosity.Normal;
            }
        }

        public NuGetLogger(ICakeLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void LogDebug(string data) => _log.Debug(data);

        public void LogVerbose(string data) => _log.Debug(data);

        public void LogInformation(string data) => _log.Debug(data);

        public void LogMinimal(string data) => _log.Information(data);

        public void LogWarning(string data) => _log.Warning(data);

        public void LogError(string data) => _log.Error(data);

        public void LogInformationSummary(string data) => _log.Information(data);

        public void LogErrorSummary(string data) => _log.Error(data);

        public void Log(LogLevel level, string data) => _log.Write(GetVerbosity(level), GetLogLevel(level), data);

        public Task LogAsync(LogLevel level, string data)
        {
            Log(level, data);
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message) => Log(message.Level, message.Message);

        public Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}
