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

                // Create the correct command and execute it.
                var command = CreateCommand(options);
                command.Execute(options);

                // Return success if we could create and run the command.
                // If the parsed options are null, consider it failed.
                return options == null ? 1 : 0;
            }
            catch (Exception ex)
            {
                if (_log.Verbosity == Verbosity.Diagnostic)
                {
                    _log.Error("Error: {0}", ex);
                }
                else
                {
                    _log.Error("Error: {0}", ex.Message);   
                }                    
                return 1;
            }
        }

        private ICommand CreateCommand(CakeOptions options)
        {
            if (options != null)
            {
                if (options.Script != null)
                {
                    if (options.ShowDescription)
                    {
                        _log.Verbosity = Verbosity.Quiet;
                        return _commandFactory.CreateDescriptionCommand();
                    }
                    return _commandFactory.CreateBuildCommand();                 
                }
                if (options.ShowHelp)
                {
                    return _commandFactory.CreateHelpCommand();
                }
                if (options.ShowVersion)
                {
                    return _commandFactory.CreateVersionCommand();
                }
            }
            return _commandFactory.CreateHelpCommand();
        }
    }
}
