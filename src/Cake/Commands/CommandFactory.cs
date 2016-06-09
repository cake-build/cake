// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Commands
{
    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly BuildCommand.Factory _buildCommandFactory;
        private readonly DebugCommand.Factory _debugCommandFactory;
        private readonly DescriptionCommand.Factory _descriptionCommandFactory;
        private readonly DryRunCommand.Factory _dryRunCommandFactory;
        private readonly HelpCommand.Factory _helpCommandFactory;
        private readonly VersionCommand.Factory _versionCommandFactory;

        public CommandFactory(
            BuildCommand.Factory buildCommandFactory,
            DebugCommand.Factory debugCommandFactory,
            DescriptionCommand.Factory descriptionCommandFactory,
            DryRunCommand.Factory dryRunCommandFactory,
            HelpCommand.Factory helpCommandFactory,
            VersionCommand.Factory versionCommandFactory)
        {
            _buildCommandFactory = buildCommandFactory;
            _debugCommandFactory = debugCommandFactory;
            _descriptionCommandFactory = descriptionCommandFactory;
            _dryRunCommandFactory = dryRunCommandFactory;
            _helpCommandFactory = helpCommandFactory;
            _versionCommandFactory = versionCommandFactory;
        }

        public ICommand CreateBuildCommand()
        {
            return _buildCommandFactory();
        }

        public ICommand CreateDebugCommand()
        {
            return _debugCommandFactory();
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
