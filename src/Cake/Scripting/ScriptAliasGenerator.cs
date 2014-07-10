using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.Extensions;
using Cake.Core.Scripting;
using Cake.Core.Scripting.CodeGen;

namespace Cake.Scripting
{
    internal sealed class ScriptAliasGenerator : IScriptAliasGenerator
    {
        private readonly ICakeLog _log;

        public ScriptAliasGenerator(ICakeLog log)
        {
            _log = log;
        }

        public void Generate(IScriptSession session, IEnumerable<Assembly> assemblies)
        {
            _log.Debug("Generating script alias code...");
            foreach (var method in ScriptAliasFinder.GetExtensionMethods(assemblies))
            {
                var signature = method.GetSignature(false);

                try
                {
                    _log.Verbose(signature);
                    session.Execute(GernerateCode(method));
                }
                catch (Exception)
                {
                    _log.Error("An error occured while generating proxy code for {0}", signature);
                    throw;
                }
            }
            _log.Debug("Done generating script alias code.");
        }

        private static string GernerateCode(MethodInfo method)
        {
            string code;
            if (method.IsDefined(typeof(CakeMethodAliasAttribute)))
            {
                code = MethodAliasGenerator.Generate(method);
            }
            else if (method.IsDefined(typeof(CakePropertyAliasAttribute)))
            {
                code = PropertyAliasGenerator.Generate(method);
            }
            else
            {
                throw new InvalidOperationException("Unknown alias type.");
            }
            return code;
        }
    }
}
