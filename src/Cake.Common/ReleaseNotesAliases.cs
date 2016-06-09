// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to release notes.
    /// </summary>
    [CakeAliasCategory("Release Notes")]
    public static class ReleaseNotesAliases
    {
        private static readonly ReleaseNotesParser _parser;

        static ReleaseNotesAliases()
        {
            _parser = new ReleaseNotesParser();
        }

        /// <summary>
        /// Parses all release notes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>All release notes.</returns>
        /// <example>
        /// <code>
        /// var releaseNotes = ParseAllReleaseNotes("./ReleaseNotes.md");
        /// foreach(var releaseNote in releaseNotes)
        /// {
        ///     Information("Version: {0}", releaseNote.Version);
        ///     foreach(var note in releaseNote.Notes)
        ///     {
        ///         Information("\t{0}", note);
        ///     }
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static IReadOnlyList<ReleaseNotes> ParseAllReleaseNotes(this ICakeContext context, FilePath filePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            if (filePath.IsRelative)
            {
                filePath = filePath.MakeAbsolute(context.Environment);
            }

            // Get the release notes file.
            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                const string format = "Release notes file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, filePath.FullPath);
                throw new CakeException(message);
            }

            using (var reader = new StreamReader(file.OpenRead()))
            {
                return _parser.Parse(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Parses the latest release notes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>The latest release notes.</returns>
        /// <example>
        /// <code>
        /// var releaseNote = ParseReleaseNotes("./ReleaseNotes.md");
        /// Information("Version: {0}", releaseNote.Version);
        /// foreach(var note in releaseNote.Notes)
        /// {
        ///     Information("\t{0}", note);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static ReleaseNotes ParseReleaseNotes(this ICakeContext context, FilePath filePath)
        {
            return ParseAllReleaseNotes(context, filePath).First();
        }
    }
}
