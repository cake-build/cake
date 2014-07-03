using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Extensions;
using Cake.Scripting.CodeGen;

namespace Cake.Scripting
{
    internal sealed class ScriptRunner : IScriptRunner
    {
        private readonly ICakeLog _log;
        private readonly IScriptSessionFactory _sessionFactory;

        public ScriptRunner(ICakeLog log, IScriptSessionFactory sessionFactory)
        {
            _log = log;
            _sessionFactory = sessionFactory;
        }

        public void Run(IScriptHost host, IEnumerable<Assembly> references, IEnumerable<string> namespaces, string code)
        {
            var session = _sessionFactory.CreateSession(host);
            var assemblies = references as Assembly[] ?? references.ToArray();

            // Add references to session.
            foreach (var reference in assemblies)
            {
                session.AddReference(reference);
            }

            // Add namespaces to session.
            foreach (var @namespace in namespaces)
            {
                session.ImportNamespace(@namespace);
            }

            // Find all extension methods and generate proxy methods.
            AddExtensionMethodsToSession(session, assemblies);

            // Execute the code.
            session.Execute(code);
        }

        private void AddExtensionMethodsToSession(IScriptSession session, IEnumerable<Assembly> assemblies)
        {
            _log.Debug("Generating script host proxy methods for extension methods...");
            foreach (var method in ScriptAliasFinder.GetExtensionMethods(assemblies))
            {
                var signature = method.GetSignature();

                try
                {
                    _log.Debug("  {0}", signature);
                    session.Execute(GenerateAliasCode(method));
                }
                catch (Exception)
                {
                    _log.Error("An error occured while generating proxy code for {0}", signature);
                    throw;
                }
            }
            _log.Debug("Done generating code.");
        }

        private static string GenerateAliasCode(MethodInfo method)
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