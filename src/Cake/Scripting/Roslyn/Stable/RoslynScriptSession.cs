﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting.Roslyn.Stable
{
    internal sealed class RoslynScriptSession : IScriptSession
    {
        private readonly Session _roslynSession;
        private readonly ICakeLog _log;
        private readonly HashSet<string> _importedNamespaces;

        public RoslynScriptSession(IScriptHost host, ICakeLog log)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            var roslynScriptEngine = new ScriptEngine();
            _roslynSession = roslynScriptEngine.CreateSession(host, typeof(IScriptHost));

            _log = log;
            _importedNamespaces = new HashSet<string>();
        }

        public void AddReferencePath(FilePath path)
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

        public void Execute(string code)
        {
            _log.Debug("Compiling build script...");
            _roslynSession.Execute(code);
        }
    }
}