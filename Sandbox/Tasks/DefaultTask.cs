using Cake.Core;
using Cake.Frosting;

namespace Sandbox.Tasks
{
    [TaskName("Default")]
    [Dependency(typeof(BuildTask))]
    public class DefaultTask : FrostingTask
    {
        // If you don't inherit from the generic task
        // the standard ICakeContext will be provided.
    }
}