namespace Cake.Commands
{
    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly BuildCommand.Factory _buildCommandFactory;
        private readonly DescriptionCommand.Factory _descriptionCommandFactory;
        private readonly DryRunCommand.Factory _dryRunCommandFactory;
        private readonly HelpCommand.Factory _helpCommandFactory;
        private readonly VersionCommand.Factory _versionCommandFactory;

        public CommandFactory(
            BuildCommand.Factory buildCommandFactory,
            DescriptionCommand.Factory descriptionCommandFactory,
            DryRunCommand.Factory dryRunCommandFactory,
            HelpCommand.Factory helpCommandFactory,
            VersionCommand.Factory versionCommandFactory)
        {
            _buildCommandFactory = buildCommandFactory;
            _descriptionCommandFactory = descriptionCommandFactory;
            _dryRunCommandFactory = dryRunCommandFactory;
            _helpCommandFactory = helpCommandFactory;
            _versionCommandFactory = versionCommandFactory;
        }

        public ICommand CreateBuildCommand()
        {
            return _buildCommandFactory();
        }

        public ICommand CreateDescriptionCommand()
        {
            return _descriptionCommandFactory();
        }

        public ICommand CreateDryRunCommand()
        {
            return _dryRunCommandFactory();
        }

        public ICommand CreateHelpCommand()
        {
            return _helpCommandFactory();
        }

        public ICommand CreateVersionCommand()
        {
            return _versionCommandFactory();
        }
    }
}
