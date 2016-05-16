namespace Cake.Core
{
    /// <summary>
    /// Represents arguments passed to script.
    /// </summary>
    public interface ICakeArguments
    {
        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        bool HasArgument(string name);

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value.</returns>
        string GetArgument(string name);
    }
}