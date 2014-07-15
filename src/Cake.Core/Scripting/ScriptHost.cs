using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    public abstract class ScriptHost : IScriptHost
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

        protected ICakeEngine Engine
        {
            get { return _engine; }
        }

        protected ScriptHost(ICakeEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            _engine = engine;
        }

        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _engine.Tasks; }
        }

        public CakeTaskBuilder<ActionTask> Task(string name)
        {
            return _engine.Task(name);
        }

        public ICakeContext GetContext()
        {
            return this;
        }

        public abstract CakeReport RunTarget(string target);
    }
}
