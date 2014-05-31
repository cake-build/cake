namespace Cake.Core
{
    public interface ICakeEngine
    {
        CakeTask Task(string name);
        void Run(string target);
    }
}
