﻿using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    public class ScriptHost : IScriptHost
    {
        protected readonly ICakeEngine _engine;

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

        public virtual CakeReport RunTarget(string target)
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
