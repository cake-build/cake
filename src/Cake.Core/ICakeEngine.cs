namespace Cake.Core
{
    public interface ICakeEngine : ICakeContext
    {
        CakeTaskBuilder<ActionTask> Task(string name);
        CakeReport RunTarget(string target);
    }
}
