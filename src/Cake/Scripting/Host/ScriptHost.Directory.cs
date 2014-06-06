using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.IO;

namespace Cake.Scripting.Host
{
    public sealed partial class ScriptHost
    {
        public void DeleteDirectory(DirectoryPath path, bool recursive = false)
        {
            ((ICakeContext)this).DeleteDirectory(path);
        }

        public void DeleteDirectories(IEnumerable<DirectoryPath> paths, bool recursive = true)
        {
            ((ICakeContext)this).DeleteDirectories(paths);
        }

        public void CleanDirectory(DirectoryPath path)
        {
            ((ICakeContext)this).CleanDirectory(path);
        }

        public void CleanDirectories(string pattern)
        {
            ((ICakeContext)this).CleanDirectories(pattern);
        }

        public void CleanDirectories(IEnumerable<DirectoryPath> paths)
        {
            ((ICakeContext)this).CleanDirectories(paths);
        }
    }
}
