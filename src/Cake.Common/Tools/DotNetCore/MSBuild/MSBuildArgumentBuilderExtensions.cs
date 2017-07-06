// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Contains functionality related to MSBuild arguments.
    /// </summary>
    public static class MSBuildArgumentBuilderExtensions
    {
        /// <summary>
        /// Adds MSBuild arguments
        /// </summary>
        /// <param name="builder">Argument builder.</param>
        /// <param name="settings">MSBuild settings to add.</param>
        /// <param name="environment">The environment.</param>
        /// <exception cref="InvalidOperationException">Throws if 10 or more file loggers specified.</exception>
        public static void AppendMSBuildSettings(this ProcessArgumentBuilder builder, DotNetCoreMSBuildSettings settings, ICakeEnvironment environment)
        {
            // Got any targets?
            if (settings.Targets.Any())
            {
                if (settings.Targets.All(string.IsNullOrWhiteSpace))
                {
                    throw new ArgumentException("Specify the name of the target", nameof(settings.Targets));
                }

                builder.AppendMSBuildSwitch("target", string.Join(";", settings.Targets));
            }

            // Got any properties?
            foreach (var property in settings.Properties)
            {
                if (property.Value == null || property.Value.All(string.IsNullOrWhiteSpace))
                {
                    throw new ArgumentException("A property must have at least one non-empty value", nameof(settings.Properties));
                }

                foreach (var value in property.Value)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    builder.AppendMSBuildSwitch("property", $"{property.Key}={value}");
                }
            }

            // Set the maximum number of processors?
            if (settings.MaxCpuCount.HasValue)
            {
                builder.AppendMSBuildSwitchWithOptionalValue("maxcpucount", settings.MaxCpuCount.Value, maxCpuCount => maxCpuCount > 0);
            }

            // use different version of msbuild?
            if (settings.ToolVersion.HasValue)
            {
                builder.AppendMSBuildSwitch("toolsversion", GetToolVersionValue(settings.ToolVersion.Value));
            }

            // configure console logger?
            if (!settings.DisableConsoleLogger && settings.ConsoleLoggerSettings != null)
            {
                var arguments = GetLoggerSettings(settings.ConsoleLoggerSettings);

                if (arguments.Any())
                {
                    builder.AppendMSBuildSwitch("consoleloggerparameters", arguments);
                }
            }

            // disable console logger?
            if (settings.DisableConsoleLogger)
            {
                builder.AppendMSBuildSwitch("noconsolelogger");
            }

            // Got any file loggers?
            if (settings.FileLoggers.Any())
            {
                if (settings.FileLoggers.Count >= 10)
                {
                    throw new InvalidOperationException("Too Many FileLoggers");
                }

                var arguments = settings
                                    .FileLoggers
                                    .Select((logger, index) => GetLoggerArgument(index, logger, environment))
                                    .Where(arg => !string.IsNullOrEmpty(arg));

                foreach (var argument in arguments)
                {
                    builder.Append(argument);
                }
            }

            // Got any distributed loggers?
            foreach (var distributedLogger in settings.DistributedLoggers)
            {
                builder.AppendMSBuildSwitch("distributedlogger", $"{GetLoggerValue(distributedLogger.CentralLogger)}*{GetLoggerValue(distributedLogger.ForwardingLogger)}");
            }

            // use a file logger for each node?
            if (settings.DistributedFileLogger)
            {
                builder.AppendMSBuildSwitch("distributedFileLogger");
            }

            // Got any loggers?
            foreach (var logger in settings.Loggers)
            {
                builder.AppendMSBuildSwitch("logger", GetLoggerValue(logger));
            }

            var showWarningsAsError = settings.TreatAllWarningsAs == MSBuildTreatAllWarningsAs.Error || settings.WarningCodesAsError.Any();
            var showWarningsAsMessages = settings.TreatAllWarningsAs == MSBuildTreatAllWarningsAs.Message || settings.WarningCodesAsMessage.Any();

            // Treat all or some warnings as errors?
            if (showWarningsAsError)
            {
                builder.AppendMSBuildSwitchWithOptionalValueIfNotEmpty("warnaserror", GetWarningCodes(settings.TreatAllWarningsAs == MSBuildTreatAllWarningsAs.Error, settings.WarningCodesAsError));
            }

            // Treat all or some warnings as messages?
            if (showWarningsAsMessages)
            {
                builder.AppendMSBuildSwitchWithOptionalValueIfNotEmpty("warnasmessage", GetWarningCodes(settings.TreatAllWarningsAs == MSBuildTreatAllWarningsAs.Message, settings.WarningCodesAsMessage));
            }

            // set project file extensions to ignore when searching for project file
            if (settings.IgnoreProjectExtensions.Any())
            {
                builder.AppendMSBuildSwitch("ignoreprojectextensions", string.Join(",", settings.IgnoreProjectExtensions));
            }

            // detailed summary?
            if (settings.DetailedSummary)
            {
                builder.AppendMSBuildSwitch("detailedsummary");
            }

            // Include response files?
            foreach (var responseFile in settings.ResponseFiles)
            {
                builder.AppendSwitchQuoted("@", string.Empty, responseFile.MakeAbsolute(environment).FullPath);
            }

            // exclude auto response files?
            if (settings.ExcludeAutoResponseFiles)
            {
                builder.AppendMSBuildSwitch("noautoresponse");
            }

            // don't output MSBuild logo?
            if (settings.NoLogo)
            {
                builder.AppendMSBuildSwitch("nologo");
            }
        }

        private static string GetLoggerValue(MSBuildLogger logger)
        {
            if (string.IsNullOrWhiteSpace(logger.Assembly))
            {
                throw new ArgumentNullException(nameof(logger.Assembly), "Assembly must be a strong name or file");
            }

            var argumentBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(logger.Class))
            {
                argumentBuilder.Append(logger.Class);
                argumentBuilder.Append(",");
                argumentBuilder.Append(logger.Assembly);
            }
            else
            {
                argumentBuilder.Append(logger.Assembly?.Quote());
            }

            if (!string.IsNullOrWhiteSpace(logger.Parameters))
            {
                argumentBuilder.Append(";");
                argumentBuilder.Append(logger.Parameters);
            }

            return argumentBuilder.ToString();
        }

        private static string GetLoggerArgument(int index, MSBuildFileLoggerSettings logger, ICakeEnvironment environment)
        {
            var parameters = GetLoggerSettings(logger, environment);
            if (string.IsNullOrWhiteSpace(parameters))
            {
                return string.Empty;
            }

            var counter = index == 0 ? string.Empty : index.ToString();
            return $"/fileLogger{counter} /fileloggerparameters{counter}:{parameters}";
        }

        private static string GetToolVersionValue(MSBuildVersion toolVersion)
        {
            switch (toolVersion)
            {
                case MSBuildVersion.MSBuild20:
                    return "2.0";
                case MSBuildVersion.MSBuild35:
                    return "3.5";
                case MSBuildVersion.MSBuild4:
                    return "4.0";
                case MSBuildVersion.MSBuild12:
                    return "12.0";
                case MSBuildVersion.MSBuild14:
                    return "14.0";
                case MSBuildVersion.MSBuild15:
                    return "15.0";
                default:
                    throw new ArgumentOutOfRangeException(nameof(toolVersion), toolVersion, "Invalid value");
            }
        }

        private static string GetLoggerSettings(MSBuildFileLoggerSettings loggerSettings, ICakeEnvironment environment)
        {
            var settings = new List<string>();

            var commonArguments = GetLoggerSettings(loggerSettings);

            if (commonArguments.Any())
            {
                settings.Add(commonArguments);
            }

            if (loggerSettings.LogFile != null)
            {
                var filePath = string.IsNullOrWhiteSpace(loggerSettings.LogFile)
                    ? string.Empty
                    : FilePath.FromString(loggerSettings.LogFile).MakeAbsolute(environment).ToString();

                settings.Add($"LogFile=\"{filePath}\"");
            }

            if (loggerSettings.AppendToLogFile)
            {
                settings.Add("Append");
            }

            if (!string.IsNullOrWhiteSpace(loggerSettings.FileEncoding))
            {
                settings.Add($"Encoding={loggerSettings.FileEncoding}");
            }

            return string.Join(";", settings);
        }

        private static string GetLoggerSettings(MSBuildLoggerSettings loggerSettings)
        {
            var settings = new List<string>();

            if (loggerSettings.PerformanceSummary)
            {
                settings.Add("PerformanceSummary");
            }

            if (loggerSettings.NoSummary)
            {
                settings.Add("NoSummary");
            }

            if (loggerSettings.SummaryOutputLevel != MSBuildLoggerOutputLevel.Default)
            {
                switch (loggerSettings.SummaryOutputLevel)
                {
                    case MSBuildLoggerOutputLevel.WarningsOnly:
                        settings.Add("WarningsOnly");
                        break;
                    case MSBuildLoggerOutputLevel.ErrorsOnly:
                        settings.Add("ErrorsOnly");
                        break;
                }
            }

            if (loggerSettings.HideItemAndPropertyList)
            {
                settings.Add("NoItemAndPropertyList");
            }

            if (loggerSettings.ShowCommandLine)
            {
                settings.Add("ShowCommandLine");
            }

            if (loggerSettings.ShowTimestamp)
            {
                settings.Add("ShowTimestamp");
            }

            if (loggerSettings.ShowEventId)
            {
                settings.Add("ShowEventId");
            }

            if (loggerSettings.ForceNoAlign)
            {
                settings.Add("ForceNoAlign");
            }

            if (loggerSettings.ConsoleColorType == MSBuildConsoleColorType.Disabled)
            {
                settings.Add("DisableConsoleColor");
            }

            if (loggerSettings.ConsoleColorType == MSBuildConsoleColorType.ForceAnsi)
            {
                settings.Add("ForceConsoleColor");
            }

            if (loggerSettings.DisableMultiprocessorLogging)
            {
                settings.Add("DisableMPLogging");
            }

            if (loggerSettings.Verbosity.HasValue)
            {
                settings.Add($"Verbosity={loggerSettings.Verbosity}");
            }

            return string.Join(";", settings);
        }

        private static string GetWarningCodes(bool shouldApplyToAllWarnings, IList<string> warningCodes)
            => shouldApplyToAllWarnings
                ? null
                : string.Join(";", warningCodes);

        private static void AppendMSBuildSwitch(this ProcessArgumentBuilder builder, string @switch)
            => builder.Append($"/{@switch}");

        private static void AppendMSBuildSwitch(this ProcessArgumentBuilder builder, string @switch, string value)
            => builder.AppendSwitch($"/{@switch}", ":", value);

        private static void AppendMSBuildSwitchQuoted(this ProcessArgumentBuilder builder, string @switch, string value)
            => builder.AppendSwitchQuoted($"/{@switch}", ":", value);

        private static void AppendMSBuildSwitchWithOptionalValueIfNotEmpty(this ProcessArgumentBuilder builder, string @switch, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                builder.AppendMSBuildSwitch(@switch, value);
                return;
            }

            builder.AppendMSBuildSwitch(@switch);
        }

        private static void AppendMSBuildSwitchWithOptionalValue<T>(this ProcessArgumentBuilder builder, string @switch, T value, Func<T, bool> predicate)
        {
            if (predicate(value))
            {
                builder.AppendMSBuildSwitch(@switch, value.ToString());
                return;
            }

            builder.AppendMSBuildSwitch(@switch);
        }
    }
}
