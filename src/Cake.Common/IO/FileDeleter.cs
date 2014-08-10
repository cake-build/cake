using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class FileDeleter
    {
        public static void DeleteFiles(ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            DeleteFiles(context, context.GetFiles(pattern));
        }

        public static void DeleteFiles(ICakeContext context, IEnumerable<FilePath> filePaths)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (filePaths == null)
            {
                throw new ArgumentNullException("filePaths");
            }

            foreach (var filePath in filePaths)
            {
                DeleteFile(context, filePath);
            }
        }

        public static void DeleteFile(ICakeContext context, FilePath filePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            filePath = filePath.MakeAbsolute(context.Environment);

            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                const string format = "The file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, filePath.FullPath);
                throw new FileNotFoundException(message, filePath.FullPath);
            }
            
            file.Delete();
        }
    }
}
