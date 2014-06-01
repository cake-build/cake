using System;
using Cake.Core.IO;

namespace Cake.Core
{
    internal sealed class CakeContext : ICakeContext
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ICakeEnvironment Environment
        {
            get { return _environment; }
        }

        public IGlobber Globber
        {
            get { return _globber; }
        }

        public CakeContext(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
        }
    }
}