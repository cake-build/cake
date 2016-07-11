// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
