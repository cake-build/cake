using Cake.Core.IO;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents the context used by the <see cref="ScriptAnalyzer"/>.
    /// </summary>
    public interface IScriptAnalyzerContext
    {
        /// <summary>
        /// Gets the current script being processed.
        /// </summary>
        /// <value>The current script being processed.</value>
        IScriptInformation Script { get; }

        /// <summary>
        /// Processes the specified script path using the same context.
        /// </summary>
        /// <param name="scriptPath">The script path to process.</param>
        void Analyze(FilePath scriptPath);

        /// <summary>
        /// Adds a script line to the result.
        /// </summary>
        /// <param name="line">The script line to add.</param>
        void AddScriptLine(string line);
    }
}