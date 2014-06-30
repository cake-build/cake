using System.Collections.Generic;

namespace Cake.Core
{
    public interface ICakeEngine : ICakeContext
    {
        IReadOnlyList<CakeTask> Tasks { get; }
        CakeTaskBuilder<ActionTask> Task(string name);
        CakeReport RunTarget(string target);
    }
}
