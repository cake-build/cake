using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.List
{
    /// <summary>
    /// The NuGet package lister used to list NuGet packages from a source.
    /// </summary>
    public sealed class NuGetList : NuGetTool<NuGetListSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        ///  Initializes a new instance of the <see cref="NuGetList"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetList(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Lists available packages with their versions
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <returns>A list of available packages</returns>
        public IEnumerable<NuGetListItem> List(NuGetListSettings settings)
        {
            return List(String.Empty, settings);
        }

        /// <summary>
        /// Lists available packages with their versions
        /// </summary>
        /// <param name="packageId">The source package id. If it equals an empty string, it will match all packageIds</param>
        /// <param name="settings">The settings</param>
        /// <returns>A list of available packages</returns>
        public IEnumerable<NuGetListItem> List(string packageId, NuGetListSettings settings)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var processSettings = new ProcessSettings
            {
                RedirectStandardOutput = true
            };

            IEnumerable<string> result = null;
            Run(settings, GetHasArguments(packageId, settings), processSettings,
                process => result = process.GetStandardOutput());

            return FilterResults(packageId, settings, result.Select(line => ConvertToNuGetListItem(line))).ToList();
        }

        private IEnumerable<NuGetListItem> FilterResults(string packageId, NuGetListSettings settings, IEnumerable<NuGetListItem> nugetListItems)
        {
            List<Func<NuGetListItem, bool>> filterPredicates = new List<Func<NuGetListItem, bool>>();

            if (settings.PackageIdComparison == PackageIdCompare.Equals)
            {
                filterPredicates.Add(item => string.Equals(item.Name, packageId, StringComparison.OrdinalIgnoreCase));
            }

            if (settings.VersionFilters?.Count > 0)
            {
                // Only one version has to be match
                filterPredicates.Add(
                    item => settings.VersionFilters.Any(
                        versionFilter =>
                            string.Equals(item.Version, versionFilter, StringComparison.OrdinalIgnoreCase)));
            }

            if (settings.FilterPredicates?.Count > 0)
            {
                filterPredicates.AddRange(settings.FilterPredicates);
            }

            return nugetListItems.Where(item => filterPredicates.All(predicate => predicate(item)));
        }

        private NuGetListItem ConvertToNuGetListItem(string line)
        {
            var splitline = line.Split(' ', '\t');
            return new NuGetListItem()
            {
                Name = splitline[0],
                Version = splitline[1]
            };
        }

        private ProcessArgumentBuilder GetHasArguments(string packageId, NuGetListSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("list");
            if (!string.IsNullOrEmpty(packageId))
            {
                builder.AppendQuoted(packageId);
            }

            if (settings.AllVersions)
            {
                builder.Append("-AllVersions");
            }

            if (settings.IncludeDelisted)
            {
                builder.Append("-IncludeDelisted");
            }

            if (settings.Prerelease)
            {
                builder.Append("-Prerelease");
            }

            if (settings.Source != null && settings.Source.Count > 0)
            {
                builder.Append("-Source");
                builder.AppendQuoted(string.Join(";", settings.Source));
            }

            if (settings.ConfigFile != null)
            {
                builder.Append("-ConfigFile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            builder.Append("-Verbosity Normal");
            builder.Append("-NonInteractive");

            return builder;
        }
    }
}
