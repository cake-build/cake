namespace Cake.Core
{
    public interface ICakeEngine : ICakeContext
    {
        CakeTask Task(string name);
        CakeReport Run(string target);
    }
}
