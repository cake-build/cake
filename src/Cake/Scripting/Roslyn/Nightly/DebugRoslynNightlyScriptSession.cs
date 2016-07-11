// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class DebugRoslynNightlyScriptSession : RoslynNightlyScriptSession
    {
        private const string CompiledType = "Submission#0";
        private const string CompiledMethod = "<Factory>";

        private readonly IScriptHost _host;
        private readonly ICakeLog _log;

        public DebugRoslynNightlyScriptSession(IScriptHost host, ICakeLog log) : base(log)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _host = host;
            _log = log;
        }

        public override void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            // Create the script options dynamically.
            var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .AddNamespaces(Namespaces)
                .AddReferences(References)
                .AddReferences(ReferencePaths.Select(r => r.FullPath));

            _log.Verbose("Compiling build script for debugging...");

            var roslynScript = Microsoft.CodeAnalysis.Scripting.CSharp.CSharpScript.Create(code, options)
                .WithGlobalsType(_host.GetType());

            var compilation = roslynScript.GetCompilation();
            compilation = compilation.WithOptions(compilation.Options
                .WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Debug)
                .WithOutputKind(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary));

            using (var exeStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var result = compilation.Emit(exeStream, pdbStream: pdbStream);

                if (result.Success)
                {
                    _log.Verbose("Compilation successful");

                    var assembly = AppDomain.CurrentDomain.Load(exeStream.ToArray(), pdbStream.ToArray());

                    var type = assembly.GetType(CompiledType);
                    var method = type.GetMethod(CompiledMethod, BindingFlags.Static | BindingFlags.Public);

                    var submissionStates = new object[2];
                    submissionStates[0] = _host;

                    method.Invoke(null, new[] { submissionStates });
                }
                else
                {
                    _log.Verbose("Compilation failed");

                    var errors = string.Join(Environment.NewLine, result.Diagnostics.Select(x => x.ToString()));
                    var message = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Error occurred when compiling: {0}", errors);

                    throw new CakeException(message);
                }
            }
        }
    }
}
