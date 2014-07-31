using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Roslyn.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSession : IScriptSession
    {
        private readonly Session _roslynSession;
        private readonly ICakeLog _log;
        private readonly HashSet<string> _importedNamespaces;

        public RoslynScriptSession(Session roslynSession, ICakeLog log)
        {
            if (roslynSession == null)
            {
                throw new ArgumentNullException("roslynSession");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _roslynSession = roslynSession;
            _log = log;
            _importedNamespaces = new HashSet<string>();
        }

        public void AddReferencePath(FilePath path)
        {
            _log.Debug("Adding reference to {0}...", path.GetFilename().FullPath);
            _roslynSession.AddReference(path.FullPath);
        }

        public void AddReference(Assembly assembly)
        {
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