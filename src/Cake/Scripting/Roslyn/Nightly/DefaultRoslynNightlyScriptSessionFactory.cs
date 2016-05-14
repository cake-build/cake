using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class DefaultRoslynNightlyScriptSessionFactory : RoslynNightlyScriptSessionFactory
    {
        public DefaultRoslynNightlyScriptSessionFactory(ICakeLog log) 
            : base(log)
        {
        }

        protected override IScriptSession CreateSession(IScriptHost host, ICakeLog log)
        {
            return new DefaultRoslynNightlyScriptSession(host, log);
        }
    }
}