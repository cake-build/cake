// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Tests.Stubs;
using Cake.Testing.Fixtures;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class DummyToolFixture : ToolFixture<DummySettings>
    {
        private readonly bool _hasDll;
        public Action<int> ExitCodeValidation { get; set; }

        public DummyToolFixture(bool hasDllExe = false)
            : base("dummy.exe", "dummy.dll")
        {
            _hasDll = hasDllExe;
            ExitCodeValidation = null;
        }

        public void GivenIsCoreClr()
        {
            Environment.SetIsCoreClr(true);
        }

        protected override void RunTool()
        {
            var tool = new DummyTool(FileSystem, Environment, ProcessRunner, Tools, ExitCodeValidation, _hasDll);
            tool.Run(Settings);
        }
    }
}