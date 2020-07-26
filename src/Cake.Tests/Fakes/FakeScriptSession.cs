using System;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Tests.Fakes
{
    public sealed class FakeScriptSession : IScriptSession
    {
        public Script ExecutedScript { get; private set; }

        public void AddReference(FilePath path)
        {
        }

        public void AddReference(Assembly assembly)
        {
        }

        public void ImportNamespace(string @namespace)
        {
        }

        public void Execute(Script script)
        {
            if (ExecutedScript != null)
            {
                throw new InvalidOperationException("Script sessions are not meant to be reused.");
            }

            ExecutedScript = script;
        }
    }
}
