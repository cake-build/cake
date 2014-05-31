using Cake.Core;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    public sealed class CakeScriptPack : ScriptPack<CakeScript>
    {
        public override IScriptPackContext GetContext()
        {
            var engine = new CakeEngine();
            return new CakeScript(engine);
        }

        public override void Initialize(IScriptPackSession session)
        {
            session.ImportNamespace("Cake.Core");
            session.ImportNamespace("Cake.Core.IO");
            session.ImportNamespace("Cake.Core.Extensions");
            session.ImportNamespace("Cake.Core.MSBuild");            
        }
    }
}