// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;

namespace Cake.NuGet
{
    internal sealed class NuGetLoadDirectiveProvider : ILoadDirectiveProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly INuGetPackageInstaller _installer;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeLog _log;

        public NuGetLoadDirectiveProvider(
            ICakeEnvironment environment,
            INuGetPackageInstaller installer,
            ICakeConfiguration configuration,
            ICakeLog log)
        {
            _environment = environment;
            _installer = installer;
            _configuration = configuration;
            _log = log;
        }

        public bool CanLoad(IScriptAnalyzerContext context, LoadReference reference)
        {
            return reference.Scheme != null && reference.Scheme.Equals("nuget", StringComparison.OrdinalIgnoreCase);
        }

        public void Load(IScriptAnalyzerContext context, LoadReference reference)
        {
            // Create a package reference from our load reference.
            // The package should contain the necessary include parameters to make sure
            // that .cake files are included as part of the result.
            var separator = reference.OriginalString.Contains("?") ? "&" : "?";
            var uri = string.Concat(reference.OriginalString, $"{separator}include=./**/*.cake");
            var package = new PackageReference(uri);

            // Find the tool folder.
            var toolPath = GetToolPath(context.Root.GetDirectory());

            // Install the NuGet package.
            var files = _installer.Install(package, PackageType.Tool, toolPath);
            if (files.Count == 0)
            {
                // No files found.
                _log.Warning("No scripts found in NuGet package {0}.", package.Package);
                return;
            }

            foreach (var file in files)
            {
                var extension = file.Path.GetExtension();
                if (extension != null && extension.Equals(".cake", StringComparison.OrdinalIgnoreCase))
                {
                    context.Analyze(file.Path);
                }
            }
        }

        private DirectoryPath GetToolPath(DirectoryPath root)
        {
            var toolPath = _configuration.GetValue("Paths_Tools");
            return !string.IsNullOrWhiteSpace(toolPath)
                ? new DirectoryPath(toolPath).MakeAbsolute(_environment)
                : root.Combine("tools");
        }
    }
}