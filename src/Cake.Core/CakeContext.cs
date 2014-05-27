using System;
using Cake.Core.IO;

namespace Cake.Core
{
    public sealed class CakeContext
    {
        private readonly IFileSystem _fileSystem;

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public CakeContext(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            _fileSystem = fileSystem;
        }
    }
}