using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Scripting
{
    public sealed class ScriptHost : ICakeEngine
    {
        private readonly ICakeEngine _engine;

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

        public ICakeLog Log
        {
            get { return _engine.Log; }
        }

        public ICakeArguments Arguments
        {
            get { return _engine.Arguments; }
        }

        public IProcessRunner ProcessRunner
        {
            get { return _engine.ProcessRunner; }
        }

        public ScriptHost(ICakeEngine engine)
        {
            _engine = engine;
        }

        public CakeTaskBuilder<ActionTask> Task(string name)
        {
            return _engine.Task(name);
        }

        public CakeReport RunTarget(string target)
        {
            var report = _engine.RunTarget(target);
            if (!report.IsEmpty)
            {
                CakeReportPrinter.Write(report);   
            }            
            return report;
        }

        public ICakeContext GetContext()
        {
            return this;
        }
    }
}
