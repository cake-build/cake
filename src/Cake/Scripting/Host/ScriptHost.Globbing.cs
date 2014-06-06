using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Scripting.Host
{
    public sealed partial class ScriptHost
    {
        public IEnumerable<DirectoryPath> GetDirectories(string pattern)
        {
            return ((ICakeContext)this).GetDirectories(pattern);
        }

        public IEnumerable<FilePath> GetFiles(string pattern)
        {
            return ((ICakeContext)this).GetFiles(pattern);
        }
    }
}