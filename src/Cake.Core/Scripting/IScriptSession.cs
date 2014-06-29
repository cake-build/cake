using System.Reflection;

namespace Cake.Core.Scripting
{
    public interface IScriptSession
    {
        void AddReference(Assembly assembly);
        void ImportNamespace(string @namespace);
        object Execute(string code);
    }
}
