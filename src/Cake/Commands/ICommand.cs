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
        void Execute(CakeOptions options);
    }
}
