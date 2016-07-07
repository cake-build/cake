﻿// Licensed to the .NET Foundation under one or more agreements.
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

        private readonly ICakeLog _log;

        private readonly DotCoverProcessRunner _runner;

        public DotCoverContext(ICakeContext context)
        {
            _context = context;
            _log = new NullLog();
            _runner = new DotCoverProcessRunner();
        }

        public FilePath FilePath
        {
            get { return _runner.FilePath; }
        }

        public ProcessSettings Settings
        {
            get { return _runner.ProcessSettings; }
        }

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

        public IToolLocator Tools
        {
            get { return _context.Tools; }
        }

        /// <summary>
        ///     Gets the logging pipeline which can be used to instantiate multiple loggers.
        /// </summary>
        public ICakeLogPipeline LogPipeline
        {
            get { return _context.LogPipeline; }
        }
    }
}