using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.OpenCover
{
    internal sealed class OpenCoverContext : ICakeContext
    {
        private readonly ICakeContext _context;
        private readonly ICakeLog _log;
        private readonly OpenCoverProcessRunner _runner;

        public IFileSystem FileSystem
        {
            get { return _context.FileSystem; }
        }

        public ICakeEnvironment Environment
        {
            get { return _context.Environment; }
        }

        public IGlobber Globber
        {
            get { return _context.Globber; }
        }

        public ICakeLog Log
        {
            get { return _log; }
        }

        public ICakeArguments Arguments
        {
            get { return _context.Arguments; }
        }

        public IProcessRunner ProcessRunner
        {
            get { return _runner; }
        }

        public IRegistry Registry
        {
            get { return _context.Registry; }
        }

        public FilePath FilePath
        {
            get { return _runner.FilePath; }
        }

        public ProcessSettings Settings
        {
            get { return _runner.ProcessSettings; }
        }

        public OpenCoverContext(ICakeContext context)
        {
            _context = context;
            _log = new NullLog();
            _runner = new OpenCoverProcessRunner();
        }
    }
}