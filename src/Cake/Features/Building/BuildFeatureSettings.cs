// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Infrastructure;

namespace Cake.Features.Building
{
    public sealed class BuildFeatureSettings : IScriptHostSettings
    {
        public BuildHostKind BuildHostKind { get; }
        public FilePath Script { get; set; }
        public Verbosity Verbosity { get; set; }
        public bool Debug { get; set; }
        public bool Exclusive { get; set; }
        public bool NoBootstrapping { get; set; }

        public BuildFeatureSettings(BuildHostKind buildHostKind)
        {
            BuildHostKind = buildHostKind;
        }
    }
}
