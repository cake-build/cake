// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSession : RoslynScriptSessionBase
    {
        private readonly IScriptHost _host;
        private readonly ICakeLog _log;

        public RoslynScriptSession(IScriptHost host, IAssemblyLoader loader, ICakeLog log)
            : base(loader, log)
        {
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
                .WithImports(Namespaces)
                .AddReferences(References)
                .AddReferences(ReferencePaths.Select(r => r.FullPath));

            _log.Verbose("Compiling build script...");
            CSharpScript.EvaluateAsync(code, options, _host).Wait();
        }
    }
}