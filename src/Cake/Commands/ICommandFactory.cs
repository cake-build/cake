namespace Cake.Commands
{
    /// <summary>
    /// Represents a command factory.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Creates the build command.
        /// </summary>
        /// <returns>The build command.</returns>
        ICommand CreateBuildCommand();

        /// <summary>
        /// Creates the description command.
        /// </summary>
        /// <returns>The description command.</returns>
        ICommand CreateDescriptionCommand();

        /// <summary>
        /// Creates the dry run command.
        /// </summary>
        /// <returns>The dry run command.</returns>
        ICommand CreateDryRunCommand();

        /// <summary>
        /// Creates the help command.
        /// </summary>
        /// <returns>The help command.</returns>
        ICommand CreateHelpCommand();

        /// <summary>
        /// Creates the version command.
        /// </summary>
        /// <returns>The version command.</returns>
        ICommand CreateVersionCommand();
    }
}