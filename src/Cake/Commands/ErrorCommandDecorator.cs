namespace Cake.Commands
{
    /// <summary>
    /// A command that decorates another command but always return failure.
    /// </summary>
    internal sealed class ErrorCommandDecorator : ICommand
    {
        private readonly ICommand _command;

        public ErrorCommandDecorator(ICommand command)
        {
            _command = command;
        }

        public bool Execute(CakeOptions options)
        {
            _command.Execute(options);
            return false;
        }
    }
}