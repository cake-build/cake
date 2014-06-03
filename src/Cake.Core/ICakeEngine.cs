using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core
{
    public interface ICakeEngine : ICakeContext
    {
        CakeTask Task(string name);
        void Run(string target);
    }
}
