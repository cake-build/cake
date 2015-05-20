namespace Cake.Core.IO
{
    /// <summary>
    ///  A fluent interface for arguments
    /// </summary>
    public interface IProcessArgumentList<T>
    {
        /// <summary>
        /// Appends an argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>Itself</returns>
        T Append(IProcessArgument argument);
    }
}
