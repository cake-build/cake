using Cake.Core;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [TaskName("Clean")]
    public sealed class CleanTask : FrostingTask<ICakeContext>
    {
    }
}