using Cake.Core;
using Cake.Core.Diagnostics;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    using Core.IO;

    public sealed class CakeScript : IScriptPackContext, ICakeEngine, ICakeContext
    {
        private readonly CakeEngine _engine;

        public IFileSystem FileSystem
        {
            get { return _engine.FileSystem; }
        }

        public ICakeEnvironment Environment
        {
            get { return _engine.Environment; }
        }

        public IGlobber Globber
        {
             get { return _engine.Globber; }
        }

        public ILogger Log
        {
            get { return _engine.Log; }
        }

        public CakeScript(CakeEngine engine)
        {
            _engine = engine;
        }

        public CakeTask Task(string name)
        {
            return _engine.Task(name);
        }

        public void Run(string target)
        {
            _engine.Run(target);
        }
    }
}
