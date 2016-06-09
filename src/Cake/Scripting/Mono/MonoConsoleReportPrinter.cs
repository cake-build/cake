// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Mono.CSharp;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoConsoleReportPrinter : ReportPrinter
    {
        private readonly ICakeLog _log;

        public MonoConsoleReportPrinter(ICakeLog log)
        {
            _log = log;
        }

        public override void Print(AbstractMessage msg, bool showFullPath)
        {
            if (msg == null)
            {
                return;
            }

            var message = GetFormattedLogMessage(msg);

            if (msg.IsWarning)
            {
                _log.Warning(message);
            }
            else if (IsError(msg))
            {
                _log.Error(message);
            }
            else
            {
                _log.Verbose(message);
            }
        }

        private static string GetFormattedLogMessage(AbstractMessage message)
        {
            var path = GetSourcePath(message);
            var row = message.Location.Row;
            var column = message.Location.Column;

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0} ({1},{2}): {3}",
                path.FullPath, row, column, message.Text);
        }

        private static FilePath GetSourcePath(AbstractMessage message)
        {
            string filename = null;

            try
            {
                if (message.Location.SourceFile != null && message.Location.SourceFile.Name != null)
                {
                    filename = message.Location.SourceFile.Name;
                }
            }
            catch
            {
                // Fix for issue #298 (https://github.com/cake-build/cake/issues/298)
                // Not pretty but it should take care of the exception being thrown
                // in certain situations when accessing the SourceFile property.
            }

            return new FilePath(filename ?? "unknown.cake").GetFilename();
        }

        private static bool IsError(AbstractMessage message)
        {
            return message.MessageType != null &&
                message.MessageType.Equals("error", StringComparison.OrdinalIgnoreCase);
        }
    }
}
