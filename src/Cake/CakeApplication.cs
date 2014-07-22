using System;
using System.Collections.Generic;
using Cake.Arguments;
using Cake.Commands;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;

namespace Cake
{
    public sealed class CakeApplication
    {
        private readonly IVerbosityAwareLog _log;
        private readonly ICommandFactory _commandFactory;
        private readonly IArgumentParser _argumentParser;

        public CakeApplication(IVerbosityAwareLog log, ICommandFactory commandFactory, IArgumentParser argumentParser)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (commandFactory == null)
            {
                throw new ArgumentNullException("commandFactory");
            }
            if (argumentParser == null)
            {
                throw new ArgumentNullException("argumentParser");
            }
            
            _log = log;
            _commandFactory = commandFactory;
            _argumentParser = argumentParser;
        }

        public int Run(IEnumerable<string> args)
       {
            try
            {
                // Parse options.
                var options = _argumentParser.Parse(args);
                if (options != null)
                {
                    _log.Verbosity = options.Verbosity;  
                }

                // CreateDefault the correct command and execute it.
                var command = CreateCommand(options);
                command.Execute(options);

                // Return success.
                return 0;
            }
            catch (Exception ex)
            {
                _log.Error("An unhandled exception occured.");
                _log.Error(ex.Message);
                return 1;
            }
        }

        private ICommand CreateCommand(CakeOptions options)
        {
            if (options != null)
            {
                if (options.ShowHelp)
                {
                    return _commandFactory.CreateHelpCommand();               
                }
                if (options.ShowVersion)
                {
                    return _commandFactory.CreateVersionCommand();
                }
                if (options.Script != null)
                {
                    if (options.ShowDescription)
                    {
                        _log.Verbosity = Verbosity.Quiet;
                        return _commandFactory.CreateDescriptionCommand();
                    }
                    return _commandFactory.CreateBuildCommand();                 
                }
            }
            return _commandFactory.CreateHelpCommand();
        }
    }
}
