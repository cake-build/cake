using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting.CodeGen;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script alias generator.
    /// </summary>
    public sealed class ScriptAliasGenerator : IScriptAliasGenerator
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAliasGenerator"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public ScriptAliasGenerator(ICakeLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Generates script aliases and adds them to the specified session.
        /// </summary>
        /// <param name="session">The session to add script aliases to.</param>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        public void Generate(IScriptSession session, IEnumerable<Assembly> assemblies)
        {
            _log.Debug("Generating script alias code...");
            foreach (var method in ScriptAliasFinder.FindAliases(assemblies))
            {
                var signature = method.GetSignature(false);

                try
                {
                    session.Execute(GenerateCode(method));
                }
                catch (Exception)
                {
                    _log.Error("An error occured while generating proxy code for {0}", signature);
                    throw;
                }
            }
            _log.Debug("Done generating script alias code.");
        }

        private static string GenerateCode(MethodInfo method)
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
