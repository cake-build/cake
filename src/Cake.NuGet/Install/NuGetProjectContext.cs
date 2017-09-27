// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Xml.Linq;
using Cake.Core.Diagnostics;
using NuGet.Packaging;
using NuGet.ProjectManagement;

namespace Cake.NuGet.Install
{
    internal sealed class NuGetProjectContext : INuGetProjectContext
    {
        private readonly ICakeLog _log;

        public NuGetProjectContext(ICakeLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            PackageExtractionContext = new PackageExtractionContext(new NuGetLogger(_log))
            {
                PackageSaveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files | PackageSaveMode.Nupkg
            };
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            switch (level)
            {
                case MessageLevel.Warning:
                    _log.Warning(message, args);
                    break;
                case MessageLevel.Error:
                    _log.Error(message, args);
                    break;
                default:
                    _log.Debug(message, args);
                    break;
            }
        }

        public void ReportError(string message)
        {
            _log.Error(message);
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            _log.Debug(message);
            return FileConflictAction.Ignore;
        }

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider => null;

        public ExecutionContext ExecutionContext => null;

        public XDocument OriginalPackagesConfig { get; set; }

        public NuGetActionType ActionType { get; set; }

        public TelemetryServiceHelper TelemetryService { get; set; }
    }
}
