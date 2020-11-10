// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
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
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _installer = installer ?? throw new ArgumentNullException(nameof(installer));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public bool CanLoad(IScriptAnalyzerContext context, LoadReference reference)
        {
            return reference.Scheme != null && reference.Scheme.Equals("nuget", StringComparison.OrdinalIgnoreCase);
        }

        public void Load(IScriptAnalyzerContext context, LoadReference reference)
        {
            // Create a package reference from our load reference.
            // If not specified in script, the package should contain the necessary include
            // parameters to make sure that .cake files are included as part of the result.
            var uri = new Uri(reference.OriginalString);
            var parameters = uri.GetQueryString();
            const string includeParameterName = "include";
            if (!parameters.ContainsKey(includeParameterName))
            {
                var separator = parameters.Count > 0 ? "&" : "?";
                uri = new Uri(string.Concat(reference.OriginalString, $"{separator}include=./**/*.cake"));
            }
            var package = new PackageReference(uri);

            // Find the tool folder.
            var toolPath = GetToolPath(_environment.WorkingDirectory);

            // Install the NuGet package.
            var files = _installer
                .Install(package, PackageType.Tool, toolPath)
                .Where(file =>
                {
                    var extension = file.Path.GetExtension();
                    return extension != null && extension.Equals(".cake", StringComparison.OrdinalIgnoreCase);
                })
                .ToArray();
            if (files.Length == 0)
            {
                // No scripts found.
                _log.Warning("No scripts found in NuGet package {0}.", package.Package);
                return;
            }

            foreach (var file in files)
            {
                context.Analyze(file.Path);
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