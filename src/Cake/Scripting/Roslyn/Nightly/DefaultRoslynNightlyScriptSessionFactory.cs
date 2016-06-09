// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class DefaultRoslynNightlyScriptSessionFactory : RoslynNightlyScriptSessionFactory
    {
        public DefaultRoslynNightlyScriptSessionFactory(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeConfiguration configuration,
            IGlobber globber,
            ICakeLog log) : base(fileSystem, environment, configuration, globber, log)
        {
        }

        protected override IScriptSession CreateSession(IScriptHost host, ICakeLog log)
        {
            // Create the session.
            return new DefaultRoslynNightlyScriptSession(host, log);
        }
    }
}
