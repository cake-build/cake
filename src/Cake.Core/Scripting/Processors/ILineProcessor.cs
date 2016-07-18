using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Represents a line processor.
    /// </summary>
    public interface ILineProcessor
    {
        /// <summary>
        /// Processes the specified line.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <param name="line">The line.</param>
        /// <param name="replacement">The replacement for line, null if no replacement</param>
        /// <returns><c>true</c> if the line was processed
        /// by this processor; otherwise <c>false</c>.</returns>
        bool Process(IScriptAnalyzerContext analyzer, string line, out string replacement);
    }
}