using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace Sandbox.Tasks
{
    [TaskName("Build")]
    public sealed class BuildTask : FrostingTask<Settings>
    {
        public override bool ShouldRun(Settings context)
        {
            // Don't run this task on OSX.
            return context.Environment.Platform.Family != PlatformFamily.OSX;
        }

        public override void Run(Settings context)
        {
            context.Log.Information("Magic: {0}", context.Magic);
        }
    }
}