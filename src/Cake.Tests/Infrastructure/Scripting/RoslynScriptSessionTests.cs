using System;
using System.Collections.Generic;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Cake.Infrastructure;
using Cake.Infrastructure.Scripting;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Infrastructure.Scripting
{
    public sealed partial class RoslynScriptSessionTests
    {
        [Fact]
        public void Generates_Equal_Hash_For_Different_Directories_Equal_Files()
        {
            var hashes = new HashSet<string>();
            var directories = new[]
            {
                "c:/source/test1",
                "c:/source/test2",
                "c:/source/test3"
            };

            foreach (var directory in directories)
            {
                var scriptHost = Substitute.For<IScriptHost>();
                scriptHost.Context.Environment.WorkingDirectory.Returns(directory);

                var assemblyLoader = Substitute.For<IAssemblyLoader>();
                var cakeConfiguration = Substitute.For<ICakeConfiguration>();
                var cakeLog = Substitute.For<ICakeLog>();

                var scriptHostSettings = Substitute.For<IScriptHostSettings>();
                scriptHostSettings.Script.Returns(new Core.IO.FilePath($"{directory}/build.cake"));

                var scriptEngine = new RoslynScriptSession(scriptHost, assemblyLoader,
                    cakeConfiguration, cakeLog, scriptHostSettings);

                var script = GenerateScript(directory);

                var hash = scriptEngine.GetScriptHash(script);
                hashes.Add(hash);
            }

            Assert.Single(hashes);
        }

        [Fact]
        public void Generates_Different_Hash_For_Equal_Directories_Different_Files()
        {
            var hashes = new HashSet<string>();

            var directory = "c:/source/test1";

            var randomLines = new[]
            {
                "random 1",
                "random 2",
                "random 3"
            };

            foreach (var randomLine in randomLines)
            {
                var scriptHost = Substitute.For<IScriptHost>();
                scriptHost.Context.Environment.WorkingDirectory.Returns(directory);

                var assemblyLoader = Substitute.For<IAssemblyLoader>();
                var cakeConfiguration = Substitute.For<ICakeConfiguration>();
                var cakeLog = Substitute.For<ICakeLog>();

                var scriptHostSettings = Substitute.For<IScriptHostSettings>();
                scriptHostSettings.Script.Returns(new Core.IO.FilePath($"{directory}/build.cake"));

                var scriptEngine = new RoslynScriptSession(scriptHost, assemblyLoader,
                    cakeConfiguration, cakeLog, scriptHostSettings);

                var script = GenerateScript(directory, new[]
                {
                    randomLine
                });

                var hash = scriptEngine.GetScriptHash(script);
                hashes.Add(hash);
            }

            Assert.Equal(randomLines.Length, hashes.Count);
        }

        private Script GenerateScript(string path, IReadOnlyList<string> additionalLines = null)
        {
            var lines = new List<string>
            {
                $"#line 1 \"{path}/build.cake\"",
                "var x = 1;",
                "var y = 2;",
                "var z = x + y;"
            };

            if (additionalLines is not null)
            {
                lines.AddRange(additionalLines);
            }

            var script = new Script(Array.Empty<string>(), lines,
                Array.Empty<ScriptAlias>(), Array.Empty<string>(), Array.Empty<string>(),
                Array.Empty<string>());

            return script;
        }
    }
}
