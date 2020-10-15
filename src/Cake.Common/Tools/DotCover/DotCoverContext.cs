// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover
{
    internal sealed class DotCoverContext : CakeContextAdapter
    {
        private readonly DotCoverProcessRunner _runner;

        public override ICakeLog Log { get; }

        public override IProcessRunner ProcessRunner => _runner;

        public FilePath FilePath => _runner.FilePath;

        public ProcessSettings Settings => _runner.ProcessSettings;

        public DotCoverContext(ICakeContext context) : base(context)
        {
            Log = new NullLog();
            _runner = new DotCoverProcessRunner();
        }
    }
}