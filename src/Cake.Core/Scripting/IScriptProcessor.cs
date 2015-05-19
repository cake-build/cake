using Cake.Core.IO;
using System;

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
        /// <param name="scriptReference">The script reference.</param>
        /// <param name="context">The context.</param>
        void Process(ScriptReference scriptReference, ScriptProcessorContext context);
    }
}
