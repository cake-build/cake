// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.SpecFlow
{
    internal sealed class SpecFlowContext : CakeContextAdapter
    {
        private readonly SpecFlowProcessRunner _runner;

        public override ICakeLog Log { get; }

        public override IProcessRunner ProcessRunner => _runner;

        public FilePath FilePath => _runner.FilePath;

        public ProcessSettings Settings => _runner.ProcessSettings;

        public SpecFlowContext(ICakeContext context) : base(context)
        {
            Log = new NullLog();
            _runner = new SpecFlowProcessRunner();
        }
    }
}