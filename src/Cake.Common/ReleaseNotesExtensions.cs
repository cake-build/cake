using System;
using System.Collections.Generic;
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
    public static class ReleaseNotesExtensions
    {
        private static readonly ReleaseNotesParser _parser;

        static ReleaseNotesExtensions()
        {
            _parser = new ReleaseNotesParser();
        }

        /// <summary>
        /// Parses all release notes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>All release notes.</returns>
        [CakeMethodAlias]
        public static IReadOnlyList<ReleaseNotes> ParseAllReleaseNotes(this ICakeContext context, FilePath filePath)
        {
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
                var message = string.Format("Release notes file '{0}' do not exist.", filePath.FullPath);
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
        [CakeMethodAlias]
        public static ReleaseNotes ParseReleaseNotes(this ICakeContext context, FilePath filePath)
        {
            return ParseAllReleaseNotes(context, filePath).First();
        }
    }
}
