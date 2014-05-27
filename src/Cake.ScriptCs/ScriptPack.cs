using Cake.Core;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    public sealed class ScriptPack : ScriptPack<ScriptPackContext>
    {
        public override IScriptPackContext GetContext()
        {
            var engine = new CakeEngine();
            return new ScriptPackContext(engine);
        }
    }
}