// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/////////////////////////////////////////////////////////////////////////////////////////////////////
// NOTE: Portions of this code was taken from the ScriptCS project
// which is licensed under the MIT license. https://github.com/scriptcs/scriptcs
/////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;

#if NETCORE
using System.Runtime.Loader;
#endif

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynDebugScriptSession : RoslynScriptSessionBase
    {
        private const string CompiledType = "Submission#0";
        private const string CompiledMethod = "<Factory>";

        private readonly IScriptHost _host;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;

        public RoslynDebugScriptSession(IScriptHost host, IAssemblyLoader loader, ICakeLog log)
            : base(loader, log)
        {
            _host = host;
            _loader = loader;
            _log = log;
        }

        public override void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            // Create the script options dynamically.
            var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .AddImports(Namespaces)
                .AddReferences(References)
                .AddReferences(ReferencePaths.Select(r => r.FullPath));

            var roslynScript = CSharpScript.Create(code, options, _host.GetType());

            var compilation = roslynScript.GetCompilation();
            compilation = compilation.WithOptions(compilation.Options
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithPlatform(Platform.AnyCpu)
                .WithOutputKind(OutputKind.DynamicallyLinkedLibrary));

            using (var assemblyStream = new MemoryStream())
            using (var symbolStream = new MemoryStream())
            {
                _log.Verbose("Compiling build script for debugging...");
                var emitOptions = new EmitOptions(false, GetDebugFormat());
                var result = compilation.Emit(assemblyStream, symbolStream, options: emitOptions);
                if (result.Success)
                {
                    // Rewind the streams.
                    assemblyStream.Seek(0, SeekOrigin.Begin);
                    symbolStream.Seek(0, SeekOrigin.Begin);

                    var assembly = _loader.LoadFromStream(assemblyStream, symbolStream);

                    var type = assembly.GetType(CompiledType);
                    var method = type.GetMethod(CompiledMethod, BindingFlags.Static | BindingFlags.Public);

                    var submissionStates = new object[2];
                    submissionStates[0] = _host;

                    method.Invoke(null, new object[] { submissionStates });
                }
                else
                {
                    var errors = string.Join(Environment.NewLine, result.Diagnostics.Select(x => x.ToString()));
                    var message = string.Format(CultureInfo.InvariantCulture, "Error occurred when compiling: {0}", errors);
                    throw new CakeException(message);
                }
            }
        }

        private static DebugInformationFormat GetDebugFormat()
        {
#if NETCORE
            return DebugInformationFormat.PortablePdb;
#else
            return DebugInformationFormat.Pdb;
#endif
        }
    }
}