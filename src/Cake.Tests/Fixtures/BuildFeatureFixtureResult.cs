using Cake.Core.Scripting;

namespace Cake.Tests.Fixtures
{
    public sealed class BuildFeatureFixtureResult
    {
        public int ExitCode { get; set; }
        public bool AttachedDebugger { get; set; }
        public Script ExecutedScript { get; set; }
    }
}
