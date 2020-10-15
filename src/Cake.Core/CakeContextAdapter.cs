// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Core
{
    /// <summary>
    /// Adapter to ensure correct conversion of Cake Context in derived classes.
    /// </summary>
    public abstract class CakeContextAdapter : ICakeContext
    {
        private readonly ICakeContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeContextAdapter"/> class.
        /// </summary>
        /// <param name="context">The Cake Context.</param>
        protected CakeContextAdapter(ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>
        /// The file system.
        /// </value>
        public virtual IFileSystem FileSystem => _context.FileSystem;

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public virtual ICakeEnvironment Environment => _context.Environment;

        /// <summary>
        /// Gets the globber.
        /// </summary>
        /// <value>
        /// The globber.
        /// </value>
        public virtual IGlobber Globber => _context.Globber;

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public virtual ICakeLog Log => _context.Log;

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public virtual ICakeArguments Arguments => _context.Arguments;

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>
        /// The process runner.
        /// </value>
        public virtual IProcessRunner ProcessRunner => _context.ProcessRunner;

        /// <summary>
        /// Gets the registry.
        /// </summary>
        /// <value>
        /// The registry.
        /// </value>
        public virtual IRegistry Registry => _context.Registry;

        /// <summary>
        /// Gets the tool locator.
        /// </summary>
        /// <value>
        /// The tool locator.
        /// </value>
        public virtual IToolLocator Tools => _context.Tools;

        /// <summary>
        /// Gets the data context resolver.
        /// </summary>
        public virtual ICakeDataResolver Data => _context.Data;

        /// <summary>
        /// Gets the cake configuration.
        /// </summary>
        public virtual ICakeConfiguration Configuration => _context.Configuration;
    }
}