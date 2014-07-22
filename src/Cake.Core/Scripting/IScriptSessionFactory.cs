namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script session factory.
    /// </summary>
    public interface IScriptSessionFactory
    {
        /// <summary>
        /// Initializes the script session factory.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Creates a new script session.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>A new script session.</returns>
        IScriptSession CreateSession(IScriptHost host);
    }
}
