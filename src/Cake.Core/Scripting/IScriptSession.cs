using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    public interface IScriptSession
    {
        void AddReferencePath(FilePath path);
        void AddReference(Assembly assembly);
        void ImportNamespace(string @namespace);
        object Execute(string code);
    }
}
