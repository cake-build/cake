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
    }
}