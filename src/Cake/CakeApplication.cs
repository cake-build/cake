// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Commands;
using Cake.Diagnostics;

namespace Cake
{
    /// <summary>
    /// The Cake application.
    /// </summary>
    internal sealed class CakeApplication
    {
        private readonly ICommandFactory _commandFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeApplication"/> class.
        /// </summary>
        /// <param name="commandFactory">The command factory.</param>
        public CakeApplication(
            ICommandFactory commandFactory)
        {
            if (commandFactory == null)
            {
                throw new ArgumentNullException("commandFactory");
            }

            _commandFactory = commandFactory;
        }

        /// <summary>
        /// Runs the application with the specified arguments.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The application exit code.</returns>
        public int Run(CakeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            // Create the correct command and execute it.
            var command = CreateCommand(options);
            var result = command.Execute(options);

            // Return success if the command succeeded.
            // If the parsed options are null, or if the command failed, consider it failed.
            return result == false ? 1 : 0;
        }

        private ICommand CreateCommand(CakeOptions options)
        {
            if (!options.HasError)
            {
                if (options.ShowHelp)
                {
                    return _commandFactory.CreateHelpCommand();
                }
                if (options.ShowVersion)
                {
                    return _commandFactory.CreateVersionCommand();
                }
                if (options.PerformDryRun)
                {
                    return _commandFactory.CreateDryRunCommand();
                }
                if (options.ShowDescription)
                {
                    return _commandFactory.CreateDescriptionCommand();
                }
                if (options.PerformDebug)
                {
                    return _commandFactory.CreateDebugCommand();
                }

                return _commandFactory.CreateBuildCommand();
            }

            return new ErrorCommandDecorator(_commandFactory.CreateHelpCommand());
        }
    }
}
