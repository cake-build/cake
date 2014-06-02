using Cake.Core;
using Cake.Core.Diagnostics;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    public sealed class CakeScriptPack : ScriptPack<CakeScript>
    {
        public override IScriptPackContext GetContext()
        {
            var log = new ConsoleLog();
            var engine = new CakeEngine(log);
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