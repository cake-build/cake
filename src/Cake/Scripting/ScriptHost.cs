using System;
using System.ComponentModel;
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

        public ScriptHost(ICakeEngine engine)
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

        public bool HasArgument(string key)
        {
            return Arguments.HasArgument(key);
        }

        public T Argument<T>(string key)
        {
            var value = Arguments.GetArgument(key);
            if (value == null)
            {
                throw new CakeException("Argument was not set.");
            }
            return Convert<T>(value);
        }

        public T Argument<T>(string key, T defaultValue)
        {
            var value = Arguments.GetArgument(key);
            return value == null 
                ? defaultValue 
                : Convert<T>(value);
        }

        private static T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof (T));
            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}
