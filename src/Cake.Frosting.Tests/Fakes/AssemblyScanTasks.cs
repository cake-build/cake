// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting.Tests;

public sealed class AssemblyScanTaskA : FrostingTask<ICakeContext>
{
    public override void Run(ICakeContext context)
    {
    }
}

public sealed class AssemblyScanTaskB : FrostingTask<ICakeContext>
{
    public override void Run(ICakeContext context)
    {
    }
}
