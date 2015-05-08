using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Implementation of <see cref="ICakeContext"/>.
    /// </summary>
    public sealed class CakeContext : ICakeContext
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;
        private readonly ICakeArguments _arguments;
        private readonly IProcessRunner _processRunner;
        private readonly ILookup<string, IToolResolver> _toolResolverLookup;
        private readonly IRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeContext"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="log">The log.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="toolResolvers">The tool resolvers.</param>
        /// <param name="registry">The registry.</param>
        public CakeContext(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log,
            ICakeArguments arguments,
            IProcessRunner processRunner,
            IEnumerable<IToolResolver> toolResolvers,
            IRegistry registry)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException("processRunner");
            }
            if (toolResolvers == null)
            {
                throw new ArgumentNullException("toolResolvers");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _log = log;
            _arguments = arguments;
            _processRunner = processRunner;

            // Create the tool resolver lookup table.
            _toolResolverLookup = toolResolvers.ToLookup(
                key => key.Name, value => value,
                StringComparer.OrdinalIgnoreCase);

            _registry = registry;
        }

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>
        /// The file system.
        /// </value>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public ICakeEnvironment Environment
        {
            get { return _environment; }
        }

        /// <summary>
        /// Gets the globber.
        /// </summary>
        /// <value>
        /// The globber.
        /// </value>
        public IGlobber Globber
        {
            get { return _globber; }
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public ICakeLog Log
        {
            get { return _log; }
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public ICakeArguments Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>
        /// The process runner.
        /// </value>
        public IProcessRunner ProcessRunner
        {
            get { return _processRunner; }
        }

        /// <summary>
        /// Gets resolver by tool name
        /// </summary>
        /// <param name="toolName">resolver tool name</param>
        /// <returns>
        /// IToolResolver for tool
        /// </returns>
        public IToolResolver GetToolResolver(string toolName)
        {
            var toolResolver = _toolResolverLookup[toolName].FirstOrDefault();
            if (toolResolver == null)
            {
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, "Failed to resolve tool: {0}", toolName));
            }
            return toolResolver;
        }

        /// <summary>
        /// Gets the registry.
        /// </summary>
        /// <value>
        /// The registry.
        /// </value>
        public IRegistry Registry
        {
            get { return _registry; }
        }
    }
}
