// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class ScriptAnalyzerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeLog Log { get; set; }
        public List<ILoadDirectiveProvider> Providers { get; set; }

        public ScriptAnalyzerFixture(bool windows = false)
        {
            Environment = windows
                ? FakeEnvironment.CreateWindowsEnvironment()
                : FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Globber = new Globber(FileSystem, Environment);
            Log = Substitute.For<ICakeLog>();
            Providers = new List<ILoadDirectiveProvider>();
        }

        public void AddFileLoadDirectiveProvider()
        {
            Providers.Add(new FileLoadDirectiveProvider(Globber, Log));
        }

        public ScriptAnalyzer CreateAnalyzer()
        {
            return new ScriptAnalyzer(FileSystem, Environment, Log, Providers);
        }

        public ScriptAnalyzerResult Analyze(FilePath script)
        {
            return CreateAnalyzer().Analyze(script, new ScriptAnalyzerSettings());
        }

        public void GivenScriptExist(FilePath path, string content)
        {
            FileSystem.CreateFile(path).SetContent(content);
        }
    }
}