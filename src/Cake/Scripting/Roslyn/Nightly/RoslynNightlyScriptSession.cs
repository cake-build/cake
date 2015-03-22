using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class RoslynNightlyScriptSession : IScriptSession
    {
        private readonly IScriptHost _host;
        private readonly ICakeLog _log;
        private readonly HashSet<FilePath> _referencePaths;
        private readonly HashSet<Assembly> _references;
        private readonly HashSet<string> _namespaces;

        public RoslynNightlyScriptSession(IScriptHost host, ICakeLog log)
        {
            _host = host;
            _log = log;

            _referencePaths = new HashSet<FilePath>(PathComparer.Default);
            _references = new HashSet<Assembly>();
            _namespaces = new HashSet<string>(StringComparer.Ordinal);
        }

        public void AddReferencePath(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _log.Debug("Adding reference to {0}...", path.GetFilename().FullPath);
            _referencePaths.Add(path);
        }

        public void AddReference(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            _log.Debug("Adding reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            _references.Add(assembly);
        }

        public void ImportNamespace(string @namespace)
        {
            if (!_namespaces.Contains(@namespace))
            {
                _log.Debug("Importing namespace {0}...", @namespace);
                _namespaces.Add(@namespace);
            }
        }

        public void Execute(string code)
        {
            // Create the script options dynamically.
            var scriptOptionsDefault = GetDefaultScriptOptionsField();
            dynamic options = scriptOptionsDefault.GetValue(null);
            options = options.AddNamespaces(_namespaces);
            options = options.AddReferences(_references);
            options = options.AddReferences(_referencePaths.Select(r => r.FullPath));

            // Execute the code.
            var csharpScriptEvalMethod = GetEvalMethod();
            _log.Debug("Compiling build script...");
            csharpScriptEvalMethod.Invoke(null, new object[] { code, options, _host });
        }

        private static Type FindType(string assemblyName, string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith(string.Concat(assemblyName, ",")))
                {
                    var type = GetLoadableTypes(assembly).FirstOrDefault(x => x.FullName == typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            const string format = "Could not find type '{0}' in assembly '{1}'.";
            throw new CakeException(string.Format(format, typeName, assemblyName));
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        private static MethodInfo GetEvalMethod()
        {
            // Get the method.
            var type = FindType("Microsoft.CodeAnalysis.Scripting.CSharp", "Microsoft.CodeAnalysis.Scripting.CSharp.CSharpScript");
            var method = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(y => y.Name == "Eval" && y.GetParameters().Length == 3);
            if (method == null)
            {
                throw new CakeException("Could not resolve method 'Microsoft.CodeAnalysis.Scripting.CSharp.CSharpScript.Eval'.");
            }
            return method;
        }

        private static FieldInfo GetDefaultScriptOptionsField()
        {
            var scriptOptionsType = FindType("Microsoft.CodeAnalysis.Scripting", "Microsoft.CodeAnalysis.Scripting.ScriptOptions");
            var scriptOptionsDefault = scriptOptionsType.GetField("Default", BindingFlags.Static | BindingFlags.Public);
            if (scriptOptionsDefault == null)
            {
                throw new CakeException("Could not resolve field 'Microsoft.CodeAnalysis.Scripting.Default'.");
            }
            return scriptOptionsDefault;
        }
    }
}
