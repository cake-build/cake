using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal abstract class RoslynNightlyScriptSessionFactory
    {
        private readonly ICakeLog _log;

        protected RoslynNightlyScriptSessionFactory(ICakeLog log)
        {
            _log = log;
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            return CreateSession(host, _log);
        }

        protected abstract IScriptSession CreateSession(IScriptHost host, ICakeLog log);
    }
}