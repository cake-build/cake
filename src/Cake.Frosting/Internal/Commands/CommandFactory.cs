// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Frosting.Internal.Composition;

namespace Cake.Frosting.Internal.Commands
{
    internal sealed class CommandFactory
    {
        private readonly Container _scope;

        public CommandFactory(Container scope)
        {
            _scope = scope;
        }

        public Command GetCommand(CakeHostOptions options)
        {
            switch (options.Command)
            {
                case CakeHostCommand.Help:
                    return _scope.Resolve<HelpCommand>();
                case CakeHostCommand.DryRun:
                    return _scope.Resolve<DryRunCommand>();
                case CakeHostCommand.Version:
                    return _scope.Resolve<VersionCommand>();
                case CakeHostCommand.Run:
                    return _scope.Resolve<RunCommand>();
                default:
                    return new ErrorDecoratorCommand(_scope.Resolve<HelpCommand>());
            }
        }
    }
}
