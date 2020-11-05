// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Features.Bootstrapping
{
    public class BootstrapFeatureSettings
    {
        public FilePath Script { get; set; }
        public Verbosity Verbosity { get; set; }
    }
}
