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

namespace Cake.Scripting.Roslyn.Stable
{
    internal sealed class DebugRoslynScriptSession : RoslynScriptSession
    {
        private const string CompiledType = "Submission#0";
        private const string CompiledMethod = "<Factory>";

        private readonly ICakeLog _log;

        public DebugRoslynScriptSession(IScriptHost host, ICakeLog log) : base(host, log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _log = log;
        }

        public override void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            _log.Verbose("Compiling build script for debugging...");

            var submission = RoslynSession.CompileSubmission<CakeReport>(code);

            var compilation = (global::Roslyn.Compilers.CSharp.Compilation)submission.Compilation;
            compilation = compilation.WithOptions(compilation.Options
                                     .WithOptimizations(false)
                                     .WithDebugInformationKind(global::Roslyn.Compilers.Common.DebugInformationKind.Full));

            using (var outputStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var result = compilation.Emit(outputStream, null, null, pdbStream);

                if (result.Success)
                {
                    _log.Verbose("Compilation successful");

                    var assembly = AppDomain.CurrentDomain.Load(outputStream.ToArray(), pdbStream.ToArray());
                    var type = assembly.GetType(CompiledType);
                    var method = type.GetMethod(CompiledMethod, BindingFlags.Static | BindingFlags.Public);

                    method.Invoke(null, new[] { RoslynSession });
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
