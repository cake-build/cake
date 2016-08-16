// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover
{
    internal sealed class DotCoverContext : ICakeContext
    {
        private readonly ICakeContext _context;
        private readonly DotCoverProcessRunner _runner;

        public IFileSystem FileSystem => _context.FileSystem;

        public ICakeEnvironment Environment => _context.Environment;

        public IGlobber Globber => _context.Globber;

        public ICakeLog Log { get; }

        public ICakeArguments Arguments => _context.Arguments;

        public IProcessRunner ProcessRunner => _runner;

        public IRegistry Registry => _context.Registry;

        public IToolLocator Tools => _context.Tools;

        public FilePath FilePath => _runner.FilePath;

        public ProcessSettings Settings => _runner.ProcessSettings;

        public DotCoverContext(ICakeContext context)
        {
            _context = context;
            Log = new NullLog();
            _runner = new DotCoverProcessRunner();
        }
    }
}