using Cake.Core;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [TaskName("Build")]
    [Dependency(typeof(CleanTask))]
    public sealed class BuildTask : FrostingTask<ICakeContext>
    {
    }
}