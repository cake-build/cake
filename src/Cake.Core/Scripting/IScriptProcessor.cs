using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    public interface IScriptProcessor
    {
        ScriptProcessorResult Process(FilePath path);
    }
}
