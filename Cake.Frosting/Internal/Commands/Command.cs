// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting.Internal.Commands
{
    internal abstract class Command
    {
        public abstract bool Execute(ICakeEngine engine, CakeHostOptions options);
    }
}
