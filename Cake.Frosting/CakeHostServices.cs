// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting.Internal;
using Cake.Frosting.Internal.Commands;

namespace Cake.Frosting
{
    internal sealed class CakeHostServices
    {
        public ICakeEnvironment Environment { get; }

        public ICakeEngine Engine { get; }

        public ICakeLog Log { get; }

        public CommandFactory CommandFactory { get; }

        public EngineInitializer EngineInitializer { get; }

        public CakeHostServices(ICakeEnvironment environment, ICakeEngine engine, ICakeLog log,
            EngineInitializer engineInitializer, CommandFactory commandFactory)
        {
            Guard.ArgumentNotNull(environment, nameof(environment));
            Guard.ArgumentNotNull(engine, nameof(engine));
            Guard.ArgumentNotNull(log, nameof(log));
            Guard.ArgumentNotNull(engineInitializer, nameof(engineInitializer));
            Guard.ArgumentNotNull(commandFactory, nameof(commandFactory));

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            if (engineInitializer == null)
            {
                throw new ArgumentNullException(nameof(engineInitializer));
            }
            if (commandFactory == null)
            {
                throw new ArgumentNullException(nameof(commandFactory));
            }

            Environment = environment;
            Engine = engine;
            Log = log;
            CommandFactory = commandFactory;
            EngineInitializer = engineInitializer;
        }
    }
}
