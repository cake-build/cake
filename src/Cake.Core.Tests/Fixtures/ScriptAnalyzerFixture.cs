// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class ScriptAnalyzerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public IProcessorExtensionFinder ProcessorExtensionFinder { get; set; }

        public ScriptAnalyzerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Log = Substitute.For<ICakeLog>();
            ProcessorExtensionFinder = Substitute.For<IProcessorExtensionFinder>();
        }

        public ScriptAnalyzer CreateAnalyzer()
        {
            return new ScriptAnalyzer(FileSystem, Environment, Log, ProcessorExtensionFinder);
        }

        public ScriptAnalyzerResult Analyze(FilePath script)
        {
            IScriptAnalyzerContext context;
            return CreateAnalyzer().Analyze(script, out context);
        }

        public void GivenScriptExist(FilePath path, string content)
        {
            FileSystem.CreateFile(path).SetContent(content);
        }
    }
}
