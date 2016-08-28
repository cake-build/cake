using Cake.Core;
using Cake.Frosting;

namespace Sandbox
{
    public class Settings : FrostingContext
    {
        public bool Magic { get; set; }

        public Settings(ICakeContext context)
            : base(context)
        {
            // You could also use a CakeLifeTime<Settings>
            // to provide a Setup method to setup the context.
            Magic = context.Arguments.HasArgument("magic");
        }
    }
}