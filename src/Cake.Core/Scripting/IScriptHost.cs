namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script host that works as a context for scripts.
    /// </summary>
    public interface IScriptHost : ICakeEngine
    {
        /// <summary>
        /// Gets the <see cref="ICakeContext"/>.
        /// </summary>
        /// <returns>The context.</returns>
        ICakeContext GetContext();
    }
}