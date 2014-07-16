using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script processor.
    /// </summary>
    public interface IScriptProcessor
    {
        /// <summary>
        /// Processes the specified script.
        /// </summary>
        /// <param name="path">The path to the script that should be processed.</param>
        /// <returns>The processing result.</returns>
        ScriptProcessorResult Process(FilePath path);
    }
}
