using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Roslyn.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSession : IScriptSession
    {
        private readonly Session _roslynSession;
        private readonly HashSet<string> _importedNamespaces;

        public RoslynScriptSession(Session roslynSession)
        {
            if (roslynSession == null)
            {
                throw new ArgumentNullException("roslynSession");
            }
            _roslynSession = roslynSession;
            _importedNamespaces = new HashSet<string>();
        }

        public void AddReferencePath(FilePath path)
        {
            _roslynSession.AddReference(path.FullPath);
        }

        public void AddReference(Assembly assembly)
        {
            _roslynSession.AddReference(assembly);
        }

        public void ImportNamespace(string @namespace)
        {
            if (!_importedNamespaces.Contains(@namespace))
            {
                _roslynSession.ImportNamespace(@namespace);
                _importedNamespaces.Add(@namespace);
            }
        }

        public void Execute(string code)
        {
            _roslynSession.Execute(code);
        }
    }
}