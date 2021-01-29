using System.Collections.Generic;
using System.Text;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Features.Building;
using Cake.Infrastructure.Composition;
using Cake.Testing;
using Cake.Tests.Fakes;
using NSubstitute;
using Spectre.Console.Cli;

namespace Cake.Tests.Fixtures
{
    public sealed class BuildFeatureFixture : IBuildFeature
    {
        public FakeFileSystem FileSystem { get; }
        public FakeEnvironment Environment { get; }
        public TestContainerConfigurator Bootstrapper { get; }
        public FakeScriptEngine ScriptEngine { get; set; }
        public FakeDebugger Debugger { get; set; }
        public FakeLog Log { get; set; }
        public FakeConsole Console { get; set; }
        public IModuleSearcher ModuleSearcher { get; set; }
        public IScriptProcessor Processor { get; set; }

        public BuildFeatureFixture(
            FakeFileSystem fileSystem = null,
            FakeEnvironment environment = null,
            TestContainerConfigurator bootstrapper = null,
            FakeLog log = null,
            FakeConsole console = null,
            IModuleSearcher moduleSearcher = null)
        {
            Environment = environment ?? FakeEnvironment.CreateUnixEnvironment();
            FileSystem = fileSystem ?? new FakeFileSystem(Environment);
            Bootstrapper = bootstrapper ?? new TestContainerConfigurator();
            ScriptEngine = new FakeScriptEngine();
            Debugger = new FakeDebugger();
            Log = log ?? new FakeLog();
            Console = console ?? new FakeConsole();
            ModuleSearcher = moduleSearcher ?? Substitute.For<IModuleSearcher>();
            Processor = Substitute.For<IScriptProcessor>();

            // Create working directory.
            FileSystem.CreateDirectory(Environment.WorkingDirectory);

            // Set the default script
            SetScript("/Working/build.cake", GetDefaultScriptContent());

            Bootstrapper.RegisterOverrides(registrar =>
            {
                registrar.RegisterInstance(FileSystem).As<IFileSystem>();
                registrar.RegisterInstance(Environment).As<ICakeEnvironment>();
                registrar.RegisterInstance(ScriptEngine).As<IScriptEngine>();
                registrar.RegisterInstance(Debugger).As<ICakeDebugger>();
                registrar.RegisterInstance(Log).As<ICakeLog>();
                registrar.RegisterInstance(Processor).As<IScriptProcessor>();
            });
        }

        public BuildFeatureFixtureResult Run(BuildFeatureSettings settings, IDictionary<string, string> arguments = null)
        {
            var remaining = new FakeRemainingArguments(arguments);
            var exitCode = ((IBuildFeature)this).Run(remaining, settings);

            return new BuildFeatureFixtureResult
            {
                ExitCode = exitCode,
                AttachedDebugger = Debugger.Attached,
                ExecutedScript = ScriptEngine.Session.ExecutedScript
            };
        }

        private void SetScript(FilePath path, string content = null)
        {
            FileSystem.CreateFile(path);
            FileSystem.GetFile(path).SetContent(content ?? string.Empty);
        }

        private string GetDefaultScriptContent()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Task(\"Default\");");
            builder.AppendLine("RunTarget(\"Default\");");
            return builder.ToString();
        }

        int IBuildFeature.Run(IRemainingArguments arguments, BuildFeatureSettings settings)
        {
            var feature = new BuildFeature(FileSystem, Environment, Bootstrapper, ModuleSearcher, Log);
            return feature.Run(arguments, settings);
        }
    }
}
