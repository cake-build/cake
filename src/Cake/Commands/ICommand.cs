namespace Cake.Commands
{
    /// <summary>
    /// Represents an executable command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command with the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns><c>true</c> if the command exited successfully; otherwise, <c>false</c>.</returns>
        bool Execute(CakeOptions options);
    }
}