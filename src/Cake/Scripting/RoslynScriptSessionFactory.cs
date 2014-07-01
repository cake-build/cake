using Cake.Core.Scripting;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting
{
    internal sealed class RoslynScriptSessionFactory : IScriptSessionFactory
    {
        public IScriptSession CreateSession(IScriptHost host)
        {
            var roslynScriptEngine = new ScriptEngine();
            var session = roslynScriptEngine.CreateSession(host, typeof(IScriptHost));
            return new RoslynScriptSession(session);
        }
    }
}
