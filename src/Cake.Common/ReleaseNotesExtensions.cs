using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common
{
    public static class ReleaseNotesExtensions
    {
        private static readonly ReleaseNotesParser _parser;

        static ReleaseNotesExtensions()
        {
            _parser = new ReleaseNotesParser();
        }

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

        [CakeMethodAlias]
        public static ReleaseNotes ParseReleaseNotes(this ICakeContext context, FilePath filePath)
        {
            return ParseAllReleaseNotes(context, filePath).First();
        }
    }
}
