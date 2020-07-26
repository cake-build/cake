// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Features.Introspection
{
    public interface IVersionFeature
    {
        int Run();
    }

    public sealed class VersionFeature : IVersionFeature
    {
        private readonly IConsole _console;
        private readonly IVersionResolver _resolver;

        public VersionFeature(IConsole console, IVersionResolver resolver)
        {
            _console = console;
            _resolver = resolver;
        }

        public int Run()
        {
            _console.WriteLine(_resolver.GetVersion());
            return 0;
        }
    }
}
