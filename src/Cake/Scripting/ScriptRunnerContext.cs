using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Scripting
{
    public sealed class ScriptRunnerContext
    {
        private readonly HashSet<FilePath> _scripts;

        public ScriptRunnerContext(PathComparer comparer)
        {
            _scripts = new HashSet<FilePath>(comparer);
        }

        public bool Exist(FilePath scriptPath)
        {
            return _scripts.Contains(scriptPath);
        }

        public void AddScript(FilePath scriptPath)
        {
            _scripts.Add(scriptPath);
        }
    }
}
