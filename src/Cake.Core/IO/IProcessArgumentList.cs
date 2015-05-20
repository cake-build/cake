namespace Cake.Core.IO
{
    /// <summary>
    ///  A fluent interface for arguments
    /// </summary>
    /// <typeparam name="T">The instance of the argument list.</typeparam>
    public interface IProcessArgumentList<T>
    {
        /// <summary>
        /// Appends an argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>
        /// The same instance so that multiple calls can be chained.
        /// </returns>
        T Append(IProcessArgument argument);
    }
}
