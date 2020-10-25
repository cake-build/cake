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

        /// <inheritdoc/>
        public virtual IFileSystem FileSystem => _context.FileSystem;

        /// <inheritdoc/>
        public virtual ICakeEnvironment Environment => _context.Environment;

        /// <inheritdoc/>
        public virtual IGlobber Globber => _context.Globber;

        /// <inheritdoc/>
        public virtual ICakeLog Log => _context.Log;

        /// <inheritdoc/>
        public virtual ICakeArguments Arguments => _context.Arguments;

        /// <inheritdoc/>
        public virtual IProcessRunner ProcessRunner => _context.ProcessRunner;

        /// <inheritdoc/>
        public virtual IRegistry Registry => _context.Registry;

        /// <inheritdoc/>
        public virtual IToolLocator Tools => _context.Tools;

        /// <inheritdoc/>
        public virtual ICakeDataResolver Data => _context.Data;

        /// <inheritdoc/>
        public virtual ICakeConfiguration Configuration => _context.Configuration;
    }
}