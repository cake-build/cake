// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.WiX.Heat
{
    /// <summary>
    /// The WiX Heat runner.
    /// </summary>
    public sealed class HeatRunner : Tool<HeatSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolService">The tool service.</param>
        public HeatRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator toolService)
            : base(fileSystem, environment, processRunner, toolService)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _environment = environment;
        }

        /// <summary>
        /// Runs the Wix Heat runner for the specified directory path.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        /// <param name="settings">The settings.</param>
        public void Run(DirectoryPath directoryPath, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (outputFile == null)
            {
                throw new ArgumentNullException(nameof(outputFile));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(directoryPath, outputFile, harvestType, settings));
        }

        /// <summary>
        /// Runs the Wix Heat runner for the specified directory path.
        /// </summary>
        /// <param name="objectFiles">The object files.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> objectFiles, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            if (objectFiles == null)
            {
                throw new ArgumentNullException(nameof(objectFiles));
            }

            if (outputFile == null)
            {
                throw new ArgumentNullException(nameof(outputFile));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var objectFilesArray = objectFiles as FilePath[] ?? objectFiles.ToArray();
            if (!objectFilesArray.Any())
            {
                throw new ArgumentException("No object files provided.", nameof(objectFiles));
            }

            Run(settings, GetArguments(objectFilesArray, outputFile, harvestType, settings));
        }

        /// <summary>
        /// Runs the Wix Heat runner for the specified directory path.
        /// </summary>
        /// <param name="harvestTarget">The harvest target.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        /// <param name="settings">The settings.</param>
        public void Run(string harvestTarget, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            if (harvestTarget == null)
            {
                throw new ArgumentNullException(nameof(harvestTarget));
            }

            if (outputFile == null)
            {
                throw new ArgumentNullException(nameof(outputFile));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(harvestTarget, outputFile, harvestType, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> objectFiles, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append(GetHarvestType(harvestType));

            // Object files
            foreach (var objectFile in objectFiles.Select(file => file.MakeAbsolute(_environment).FullPath))
            {
                builder.AppendQuoted(objectFile);
            }

            var args = GetArguments(outputFile, settings);

            args.CopyTo(builder);

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath directoryPath, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append(GetHarvestType(harvestType));
            builder.AppendQuoted(directoryPath.MakeAbsolute(_environment).FullPath);

            var args = GetArguments(outputFile, settings);

            args.CopyTo(builder);

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(string harvestTarget, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append(GetHarvestType(harvestType));

            builder.AppendQuoted(harvestTarget);

            var args = GetArguments(outputFile, settings);

            args.CopyTo(builder);

            return builder;
        }

        private ProcessArgumentBuilder GetArguments(FilePath outputFile, HeatSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add extensions
            if (settings.Extensions != null && settings.Extensions.Any())
            {
                var extensions = settings.Extensions.Select(extension => string.Format(CultureInfo.InvariantCulture, "-ext {0}", extension));
                foreach (var extension in extensions)
                {
                    builder.Append(extension);
                }
            }

            // No logo
            if (settings.NoLogo)
            {
                builder.Append("-nologo");
            }

            // Suppress specific warnings
            if (settings.SuppressSpecificWarnings != null && settings.SuppressSpecificWarnings.Any())
            {
                var warnings = settings.SuppressSpecificWarnings.Select(warning => string.Format(CultureInfo.InvariantCulture, "-sw{0}", warning));
                foreach (var warning in warnings)
                {
                    builder.Append(warning);
                }
            }

            // Treat specific warnings as errors
            if (settings.TreatSpecificWarningsAsErrors != null && settings.TreatSpecificWarningsAsErrors.Any())
            {
                var errors = settings.TreatSpecificWarningsAsErrors.Select(error => string.Format(CultureInfo.InvariantCulture, "-wx{0}", error));
                foreach (var error in errors)
                {
                    builder.Append(error);
                }
            }

            // Auto generate guids
            if (settings.AutogeneratedGuid)
            {
                builder.Append("-ag");
            }

            // Component group name
            if (settings.ComponentGroupName != null)
            {
                builder.Append("-cg");
                builder.Append(settings.ComponentGroupName);
            }

            if (!string.IsNullOrEmpty(settings.Configuration))
            {
                builder.Append("-configuration");
                builder.Append(settings.Configuration);
            }

            // Directory reference id
            if (!string.IsNullOrEmpty(settings.DirectoryId))
            {
                builder.Append("-directoryid");
                builder.Append(settings.DirectoryId);
            }

            if (!string.IsNullOrWhiteSpace(settings.DirectoryReferenceId))
            {
                builder.Append("-dr");
                builder.Append(settings.DirectoryReferenceId);
            }

            // Default is components
            if (settings.Generate != WiXGenerateType.Components)
            {
                builder.Append("-generate");
                builder.Append(settings.Generate.ToString().ToLower());
            }

            if (settings.GenerateGuid)
            {
                builder.Append("-gg");
            }

            if (settings.GenerateGuidWithoutBraces)
            {
                builder.Append("-g1");
            }

            if (settings.KeepEmptyDirectories)
            {
                builder.Append("-ke");
            }

            if (!string.IsNullOrEmpty(settings.Platform))
            {
                builder.Append("-platform");
                builder.Append(settings.Platform);
            }

            if (settings.OutputGroup != null)
            {
                builder.Append("-pog:");
                switch (settings.OutputGroup)
                {
                    case WiXOutputGroupType.Binaries:
                        builder.Append("binaries");
                        break;
                    case WiXOutputGroupType.Symbols:
                        builder.Append("symbols");
                        break;
                    case WiXOutputGroupType.Documents:
                        builder.Append("documents");
                        break;
                    case WiXOutputGroupType.Satallites:
                        builder.Append("satallites");
                        break;
                    case WiXOutputGroupType.Sources:
                        builder.Append("sources");
                        break;
                    case WiXOutputGroupType.Content:
                        builder.Append("content");
                        break;
                }
            }

            if (!string.IsNullOrEmpty(settings.ProjectName))
            {
                builder.Append("-projectname");
                builder.Append(settings.ProjectName);
            }

            // Suppress Com
            if (settings.SuppressCom)
            {
                builder.Append("-scom");
            }

            // Suppress fragments
            if (settings.SuppressFragments)
            {
                builder.Append("-sfrag");
            }

            // Suppress unique identifiers
            if (settings.SuppressUniqueIds)
            {
                builder.Append("-suid");
            }

            // Suppress root directory
            if (settings.SuppressRootDirectory)
            {
                builder.Append("-srd");
            }

            // Suppress root directory
            if (settings.SuppressRegistry)
            {
                builder.Append("-sreg");
            }

            if (settings.SuppressVb6Com)
            {
                builder.Append("-svb6");
            }

            if (settings.Template != null)
            {
                builder.Append("-template");

                switch (settings.Template)
                {
                    case WiXTemplateType.Fragment:
                        builder.Append("fragment");
                        break;
                    case WiXTemplateType.Module:
                        builder.Append("module");
                        break;
                    case WiXTemplateType.Product:
                        builder.Append("product");
                        break;
                }
            }

            if (settings.Transform != null)
            {
                builder.Append("-t");
                builder.AppendQuoted(settings.Transform);
            }

            if (settings.Indent != null)
            {
                builder.Append("-indent");
                builder.Append(settings.Indent.ToString());
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Preprocessor variable
            if (!string.IsNullOrEmpty(settings.PreprocessorVariable))
            {
                builder.Append("-var");
                builder.Append(settings.PreprocessorVariable);
            }

            // Generate binder variables
            if (settings.GenerateBinderVariables)
            {
                builder.Append("-wixvar");
            }

            // Output file
            builder.Append("-out");
            builder.AppendQuoted(outputFile.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        private string GetHarvestType(WiXHarvestType harvestType)
        {
            switch (harvestType)
            {
                case WiXHarvestType.Dir:
                    return "dir";
                case WiXHarvestType.File:
                    return "file";
                case WiXHarvestType.Project:
                    return "project";
                case WiXHarvestType.Reg:
                    return "reg";
                case WiXHarvestType.Perf:
                    return "perf";
                case WiXHarvestType.Website:
                    return "website";
                default:
                    return "dir";
            }
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns> The name of the tool. </returns>
        protected override string GetToolName()
        {
            return "Heat";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns> The tool executable name. </returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "heat.exe" };
        }
    }
}