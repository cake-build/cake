using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Core
{
    public interface ICakeEngine
    {
        CakeTask Task(string name);
        void Run(string target);
    }
}
