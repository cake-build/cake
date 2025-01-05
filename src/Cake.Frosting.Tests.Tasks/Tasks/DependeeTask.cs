using Cake.Core;

namespace Cake.Frosting.Tests
{
    [IsDependeeOf(typeof(CleanTask))]
    public sealed class DependeeTask : FrostingTask<ICakeContext>
    {
    }
}