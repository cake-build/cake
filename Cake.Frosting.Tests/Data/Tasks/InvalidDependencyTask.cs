using System;
using Cake.Core;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [Dependency(typeof(DateTime))]
    public sealed class InvalidDependencyTask : FrostingTask<ICakeContext>
    {
    }
}
