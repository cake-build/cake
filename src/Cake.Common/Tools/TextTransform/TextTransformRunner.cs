﻿using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.TextTransform
{
    /// <summary>
    /// The Text Transform runner.
    /// </summary>
    public sealed class TextTransformRunner : Tool<TextTransformSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Runs Text Transform with the specified files and settings.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath sourceFile, TextTransformSettings settings)
        {
            if (sourceFile == null)
            {
                throw new ArgumentNullException("sourceFile");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(sourceFile, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath sourceFilePath, TextTransformSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.OutputFile != null)
            {
                builder.Append("-out");
                builder.AppendQuoted(settings.OutputFile.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrEmpty(settings.Assembly))
            {
                builder.Append("-r");
                builder.Append(settings.Assembly);
            }

            if (!string.IsNullOrEmpty(settings.Namespace))
            {
                builder.Append("-u");
                builder.Append(settings.Namespace);
            }

            if (settings.ReferencePath != null)
            {
                builder.Append("-P");
                builder.AppendQuoted(settings.ReferencePath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.IncludeDirectory != null)
            {
                builder.Append("-I");
                builder.AppendQuoted(settings.IncludeDirectory.MakeAbsolute(_environment).FullPath);
            }

            builder.AppendQuoted(sourceFilePath.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public TextTransformRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IGlobber globber) : base(fileSystem, environment, processRunner, globber)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            _environment = environment;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "TextTransform";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "TextTransform.exe" };
        }
    }
}