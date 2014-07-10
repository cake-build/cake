using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class DirectoryCreator
    {
        public static void Create(ICakeContext context, DirectoryPath path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.IsRelative)
            {
                path = path.MakeAbsolute(context.Environment);
            }

            var directory = context.FileSystem.GetDirectory(path);
            if (!directory.Exists)
            {                
                directory.Create();
            }
        }
    }
}
