namespace Cake.Commands
{
    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly BuildCommand.Factory _buildCommandFactory;
        private readonly DescriptionCommand.Factory _descriptionCommandFactory;
        private readonly HelpCommand.Factory _helpCommandFactory;        

        public CommandFactory(BuildCommand.Factory buildCommandFactory,
            DescriptionCommand.Factory descriptionCommandFactory, HelpCommand.Factory helpCommandFactory)
        {
            _buildCommandFactory = buildCommandFactory;
            _descriptionCommandFactory = descriptionCommandFactory;
            _helpCommandFactory = helpCommandFactory;            
        }

        public ICommand CreateBuildCommand()
        {
            return _buildCommandFactory();
        }

        public ICommand CreateDescriptionCommand()
        {
            return _descriptionCommandFactory();
        }

        public ICommand CreateHelpCommand()
        {
            return _helpCommandFactory();
        }
    }
}
