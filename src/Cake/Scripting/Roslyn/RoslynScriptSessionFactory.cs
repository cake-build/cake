using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSessionFactory : IScriptSessionFactory
    {
        private readonly ICakeEnvironment _environment;
        private readonly IRoslynInstaller _installer;
        private readonly ICakeLog _log;

        public RoslynScriptSessionFactory(ICakeEnvironment environment,
            IRoslynInstaller installer, ICakeLog log)
        {
            _environment = environment;
            _installer = installer;
            _log = log;
        }

        public void Initialize()
        {
            var root = _environment.GetApplicationRoot();
            if (!_installer.IsInstalled(root))
            {
                _log.Information("Downloading and installing Roslyn...");
                _installer.Install(root);
            }  
        }

        public IScriptSession CreateSession(IScriptHost host)
        {
            _log.Debug("Creating script session...");
            var roslynScriptEngine = new ScriptEngine();
            var session = roslynScriptEngine.CreateSession(host, typeof(IScriptHost));
            return new RoslynScriptSession(session, _log);
        }
    }
}
