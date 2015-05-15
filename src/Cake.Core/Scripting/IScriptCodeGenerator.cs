using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Generates code.
    /// </summary>
    public interface IScriptCodeGenerator
    {
        /// <summary>
        /// Generates script engine specific code from the provided context.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>The generated code.</returns>
        string Generate(Script script);
    }
}
