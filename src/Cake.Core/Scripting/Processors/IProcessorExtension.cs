namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Extends a processor alias.
    /// </summary>
    public interface IProcessorExtension : ILineProcessor
    {
        /// <summary>
        /// Determind if this <see cref="IProcessorExtension"/> can process the directive <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">directive processor alias</param>
        /// <param name="value">the alias value</param>
        /// <returns>True if <see cref="IProcessorExtension"/> can process this <paramref name="alias"/>, else False</returns>
        bool CanProcessDirective(string alias, string value);

        /// <summary>
        /// Defines the <see cref="IScriptRunnerExtension"/> containing installation instructions.
        /// </summary>
        IScriptRunnerExtension ScriptRunnerExtension { get; }
    }
}