// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Stable
{
    internal abstract class RoslynScriptSession : IScriptSession
    {
        private readonly global::Roslyn.Scripting.Session _roslynSession;
        private readonly ICakeLog _log;
        private readonly HashSet<string> _importedNamespaces;

        protected RoslynScriptSession(IScriptHost host, ICakeLog log)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            var roslynScriptEngine = new global::Roslyn.Scripting.CSharp.ScriptEngine();
            _roslynSession = roslynScriptEngine.CreateSession(host, typeof(IScriptHost));

            _log = log;
            _importedNamespaces = new HashSet<string>();
        }

        protected global::Roslyn.Scripting.Session RoslynSession
        {
            get
            {
                return _roslynSession;
            }
        }

        public void AddReference(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _log.Debug("Adding reference to {0}...", path.GetFilename().FullPath);
            _roslynSession.AddReference(path.FullPath);
        }

        public void AddReference(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            _log.Debug("Adding reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            _roslynSession.AddReference(assembly);
        }

        public void ImportNamespace(string @namespace)
        {
            if (!_importedNamespaces.Contains(@namespace))
            {
                _log.Debug("Importing namespace {0}...", @namespace);
                _roslynSession.ImportNamespace(@namespace);
                _importedNamespaces.Add(@namespace);
            }
        }

        public abstract void Execute(Script script);
    }
}
