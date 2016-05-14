using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class DebugRoslynNightlyScriptSessionFactory : RoslynNightlyScriptSessionFactory
    {
        public DebugRoslynNightlyScriptSessionFactory(ICakeLog log) 
            : base(log)
        {
        }

        protected override IScriptSession CreateSession(IScriptHost host, ICakeLog log)
        {
            return new DebugRoslynNightlyScriptSession(host, log);
        }
    }
}