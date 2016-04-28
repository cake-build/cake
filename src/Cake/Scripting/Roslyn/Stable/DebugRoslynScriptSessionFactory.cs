using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Stable
{
    internal sealed class DebugRoslynScriptSessionFactory : RoslynScriptSessionFactory
    {
        public DebugRoslynScriptSessionFactory(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeConfiguration configuration,
            ICakeLog log) : base(fileSystem, environment, configuration, log)
        {
        }

        protected override IScriptSession CreateSession(IScriptHost host, ICakeLog log)
        {
            // Create a new session.
            return new DebugRoslynScriptSession(host, log);
        }
    }
}