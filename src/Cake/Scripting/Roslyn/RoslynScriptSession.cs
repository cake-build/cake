using System;
using System.Reflection;
using Cake.Core.Scripting;
using Roslyn.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSession : IScriptSession
    {
        private readonly Session _roslynSession;

        public RoslynScriptSession(Session roslynSession)
        {
            if (roslynSession == null)
            {
                throw new ArgumentNullException("roslynSession");
            }
            _roslynSession = roslynSession;
        }

        public void AddReference(Assembly assembly)
        {
            _roslynSession.AddReference(assembly);
        }

        public void ImportNamespace(string @namespace)
        {
            _roslynSession.ImportNamespace(@namespace);
        }

        public object Execute(string code)
        {
            return _roslynSession.Execute(code);
        }
    }
}