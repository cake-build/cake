using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    public sealed class CakeScriptPack : ScriptPack<CakeScript>
    {
        public override IScriptPackContext GetContext()
        {
            var fileSystem = new FileSystem();
            var environment = new CakeEnvironment();
            var log = new ConsoleLog();
            var globber = new Globber(fileSystem, environment);
            var engine = new CakeEngine(fileSystem, environment, log, globber);
            return new CakeScript(engine);
        }

        public override void Initialize(IScriptPackSession session)
        {
            session.ImportNamespace("Cake.Core");
            session.ImportNamespace("Cake.Core.IO");
            session.ImportNamespace("Cake.Core.Extensions");
            session.ImportNamespace("Cake.Core.MSBuild");
            session.ImportNamespace("Cake.Core.XUnit");
            session.ImportNamespace("Cake.Core.Diagnostics");
        }
    }
}