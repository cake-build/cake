// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script alias finder.
    /// </summary>
    public sealed class ScriptAliasFinder : IScriptAliasFinder
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAliasFinder"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public ScriptAliasFinder(ICakeLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Finds script aliases in the provided assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        /// <returns>The script aliases that were found.</returns>
        public IReadOnlyList<ScriptAlias> FindAliases(IEnumerable<Assembly> assemblies)
        {
            var result = new List<ScriptAlias>();
            if (assemblies != null)
            {
                foreach (var reference in assemblies)
                {
                    try
                    {
                        foreach (var type in reference.DefinedTypes)
                        {
                            if (type.IsStatic())
                            {
                                foreach (var method in GetAliasMethods(type))
                                {
                                    var alias = CreateAlias(method);
                                    if (alias != null)
                                    {
                                        result.Add(alias);
                                    }
                                }
                            }
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        HashSet<string> notFound = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (Exception loaderException in ex.LoaderExceptions)
                        {
                            _log.Debug(loaderException.Message);
                            FileNotFoundException fileNotFoundException = loaderException as FileNotFoundException;
                            if (fileNotFoundException != null)
                            {
                                if (!notFound.Contains(fileNotFoundException.FileName))
                                {
                                    notFound.Add(fileNotFoundException.FileName);
                                }

                                if (!string.IsNullOrEmpty(fileNotFoundException.FusionLog))
                                {
                                    _log.Debug("Fusion Log:");
                                    _log.Debug(fileNotFoundException.FusionLog);
                                }
                            }
                            _log.Debug(string.Empty);
                        }

                        foreach (var file in notFound)
                        {
                            _log.Warning("Could not load {0} (missing {1})", reference.Location, file);
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        _log.Warning("Could not load {0} (missing {1}))", reference.Location, ex.FileName);
                    }
                }
            }
            return result;
        }

        private ScriptAlias CreateAlias(Tuple<MethodInfo, ScriptAliasType> alias)
        {
            var method = alias.Item1;
            var type = alias.Item2;

            try
            {
                var namespaces = new HashSet<string>(StringComparer.Ordinal);

                // Import the method's namespace to the session.
                var methodNamespace = method.GetNamespace();
                namespaces.Add(methodNamespace);

                // Find out if the method want us to import more namespaces.
                var namespaceAttributes = method.GetCustomAttributes<CakeNamespaceImportAttribute>();
                foreach (var namespaceAttribute in namespaceAttributes)
                {
                    namespaces.Add(namespaceAttribute.Namespace);
                }

                // Find out if the class contains any more namespaces.
                namespaceAttributes = method.DeclaringType.GetTypeInfo().GetCustomAttributes<CakeNamespaceImportAttribute>();
                foreach (var namespaceAttribute in namespaceAttributes)
                {
                    namespaces.Add(namespaceAttribute.Namespace);
                }

                return new ScriptAlias(method, type, namespaces);
            }
            catch (Exception ex)
            {
                // Log this error.
                const string format = "An error occured while generating code for alias {0}.";
                _log.Error(format, method.GetSignature(false));
                _log.Error("Error: {0}", ex.Message);
            }

            return null;
        }

        private static IEnumerable<Tuple<MethodInfo, ScriptAliasType>> GetAliasMethods(Type type)
        {
            var methods = type.GetTypeInfo().DeclaredMethods;
            foreach (var method in methods)
            {
                if (!method.IsDefined(typeof(ExtensionAttribute)))
                {
                    continue;
                }

                var scriptAliasType = GetScriptAliasType(method);
                if (scriptAliasType == ScriptAliasType.Unknown)
                {
                    continue;
                }

                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    continue;
                }

                if (parameters[0].ParameterType == typeof(ICakeContext))
                {
                    yield return Tuple.Create(method, scriptAliasType);
                }
            }
        }

        private static ScriptAliasType GetScriptAliasType(MethodInfo method)
        {
            if (method.IsDefined(typeof(CakeMethodAliasAttribute)))
            {
                return ScriptAliasType.Method;
            }
            if (method.IsDefined(typeof(CakePropertyAliasAttribute)))
            {
                return ScriptAliasType.Property;
            }
            return ScriptAliasType.Unknown;
        }
    }
}
