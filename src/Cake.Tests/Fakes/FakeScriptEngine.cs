using Cake.Core.Scripting;

namespace Cake.Tests.Fakes
{
    public sealed class FakeScriptEngine : IScriptEngine
    {
        public IScriptHost ScriptHost { get; set; }
        public FakeScriptSession Session { get; }

        public FakeScriptEngine()
        {
            Session = new FakeScriptSession();
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            ScriptHost = host;
            return Session;
        }
    }
}
