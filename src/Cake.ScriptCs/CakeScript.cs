using Cake.Core;
using ScriptCs.Contracts;

namespace Cake.ScriptCs
{
    using Core.IO;

    public sealed class CakeScript : IScriptPackContext, ICakeEngine
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
