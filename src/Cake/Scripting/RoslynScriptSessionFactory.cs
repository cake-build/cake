using Cake.Core.Scripting;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting
{
    internal sealed class RoslynScriptSessionFactory : IScriptSessionFactory
    {
        private readonly ScriptEngine _roslynScriptEngine;

        public RoslynScriptSessionFactory()
        {
            _roslynScriptEngine = new ScriptEngine();
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            var session = _roslynScriptEngine.CreateSession(host, typeof(IScriptHost));
            return new RoslynScriptSession(session);
        }
    }
}
