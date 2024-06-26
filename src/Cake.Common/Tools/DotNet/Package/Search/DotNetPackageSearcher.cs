// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Package.Search
{
    /// <summary>
    /// .NET package searcher.
    /// </summary>
    public sealed class DotNetPackageSearcher : DotNetTool<DotNetPackageSearchSettings>
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetPackageSearcher" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetPackageSearcher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Searches for packages.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="settings">The search settings.</param>
        /// <returns>A collection of <see cref="DotNetPackageSearchItem"/>.</returns>
        public IEnumerable<DotNetPackageSearchItem> Search(string searchTerm, DotNetPackageSearchSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var processSettings = new ProcessSettings
            {
                RedirectStandardOutput = true
            };

            using var ms = new MemoryStream();
            RunCommand(
                settings,
                GetArguments(searchTerm, settings),
                processSettings,
                process =>
                {
                    using var sr = new StreamWriter(ms, leaveOpen: true);
                    foreach (var line in process.GetStandardOutput())
                    {
                        sr.WriteLine(line);
                    }
                });

            return Parse(ms.GetBuffer().AsMemory(0, (int)ms.Length)).ToList();
        }

        private ProcessArgumentBuilder GetArguments(string searchTerm, DotNetPackageSearchSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("package search");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                builder.AppendQuoted(searchTerm);
            }

            if (settings.Prerelease)
            {
                builder.Append("--prerelease");
            }

            if (settings.ExactMatch)
            {
                builder.Append("--exact-match");
            }

            if (settings.Sources != null && settings.Sources.Count > 0)
            {
                foreach (var source in settings.Sources)
                {
                    builder.Append("--source");
                    builder.AppendQuoted(source);
                }
            }

            if (settings.ConfigFile != null)
            {
                builder.Append("--configfile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Take is { } take)
            {
                builder.Append("--take");
                builder.Append(take.ToString());
            }

            if (settings.Skip is { } skip)
            {
                builder.Append("--skip");
                builder.Append(skip.ToString());
            }

            builder.Append("--verbosity normal");

            builder.Append("--format json");

            return builder;
        }

        private static IEnumerable<DotNetPackageSearchItem> Parse(ReadOnlyMemory<byte> json)
        {
            var result = JsonSerializer.Deserialize<Result>(json.Span, _jsonSerializerOptions);

            if (result is not null)
            {
                foreach (var searchResult in result.SearchResult)
                {
                    foreach (var package in searchResult.Packages)
                    {
                        yield return new DotNetPackageSearchItem { Name = package.Id, Version = package.LatestVersion };
                    }
                }
            }
        }

        private sealed class Result
        {
            public List<SearchResult> SearchResult { get; set; }
        }

        private sealed class SearchResult
        {
            public List<Package> Packages { get; set; }
        }

        private sealed class Package
        {
            public string Id { get; set; }

            public string LatestVersion { get; set; }
        }
    }
}
