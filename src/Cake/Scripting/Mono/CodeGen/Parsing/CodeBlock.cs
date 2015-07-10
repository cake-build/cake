using System.Diagnostics;

namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    [DebuggerDisplay("{Content, nq}")]
    internal sealed class CodeBlock
    {
        public bool HasScope { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public string Content { get; set; }
    }
}