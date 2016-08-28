using Cake.Core;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [TaskName("Run-Unit-Tests")]
    [Dependency(typeof(BuildTask))]
    public sealed class UnitTestsTask : FrostingTask<ICakeContext>
    {
    }
}